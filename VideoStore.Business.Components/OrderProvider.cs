using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Business.Components.Interfaces;
using VideoStore.Business.Entities;
using System.Transactions;
using Microsoft.Practices.ServiceLocation;
using DeliveryCo.MessageTypes;

using System.Threading;
using VideoStore.Business.Components.PublisherService;

namespace VideoStore.Business.Components
{
    public class OrderProvider : IOrderProvider
    {
        //public IEmailProvider EmailProvider
        //{
        //    get { return ServiceLocator.Current.GetInstance<IEmailProvider>(); }
        //}

        public IUserProvider UserProvider
        {
            get { return ServiceLocator.Current.GetInstance<IUserProvider>(); }
        }

        public void SubmitOrder(Entities.Order pOrder)
        {
           
            using (TransactionScope lScope = new TransactionScope())
            {
                LoadMediaStocks(pOrder);
                MarkAppropriateUnchangedAssociations(pOrder);
                using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
                {
                    try
                    {
                        pOrder.OrderNumber = Guid.NewGuid();
                        lContainer.Orders.ApplyChanges(pOrder);
                        lContainer.SaveChanges();
                       
                        TransferFundsFromCustomer(UserProvider.ReadUserById(pOrder.Customer.Id).BankAccountNumber, pOrder.Total ?? 0.0,pOrder.OrderNumber);
                    //Stop here. Wait for bank's message to continue
                    

                }
                    catch (Exception lException)
                    {
                        SendOrderErrorMessage(pOrder, lException);
                        //throw;
                    }
                }
                lScope.Complete();
            }
            SendOrderPlacedConfirmation(pOrder);
        }

        public void AfterTransferResultReturns(Boolean Success,Guid pOrderNumber,String pMsg)
        {//continue delivery and emailing after received bank's transfer message
            
                using (TransactionScope lScope = new TransactionScope())
                {
                    using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
                    {
                        Entities.Order pOrder = lContainer.Orders.Include("Delivery").Include("OrderItems.Media").Include("Customer")
                            .Where((pOrder1) => pOrder1.OrderNumber == pOrderNumber).FirstOrDefault();
                        foreach(Entities.OrderItem oi in pOrder.OrderItems)
                        {
                        Entities.Media media = oi.Media;
                        Entities.Stock stock = lContainer.Stocks.Where((p) => p.Media.Id == media.Id).FirstOrDefault();
                        media.Stocks = stock;
                        }
                    if (Success)
                    {
                        pOrder.UpdateStockLevels();
                        PlaceDeliveryForOrder(pOrder);
                        lContainer.Orders.ApplyChanges(pOrder);
                        lContainer.SaveChanges();                    
                    }
                    else
                    {
                        Common.Model.SendEmailMessage emailMessage = new Common.Model.SendEmailMessage()
                        {
                            Topic = "Email",
                            Message = pMsg+" Order number: " + pOrderNumber + ".",
                            ToAddresses = pOrder.Customer.Email,
                            Date = new DateTime()
                        };

                        PublisherServiceClient lClient = new PublisherServiceClient();
                        lClient.Publish(emailMessage);
                    }
                    lScope.Complete();
                }
                }
            
        }

        private void MarkAppropriateUnchangedAssociations(Order pOrder)
        {
            pOrder.Customer.MarkAsUnchanged();
            pOrder.Customer.LoginCredential.MarkAsUnchanged();
            foreach (OrderItem lOrder in pOrder.OrderItems)
            {
                lOrder.Media.Stocks.MarkAsUnchanged();
                lOrder.Media.MarkAsUnchanged();
            }
        }

        private void LoadMediaStocks(Order pOrder)
        {
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                foreach (OrderItem lOrder in pOrder.OrderItems)
                {
                    lOrder.Media.Stocks = lContainer.Stocks.Where((pStock) => pStock.Media.Id == lOrder.Media.Id).FirstOrDefault();    
                }
            }
        }

        

        private void SendOrderErrorMessage(Order pOrder, Exception pException)
        {
            //EmailProvider.SendMessage(new EmailMessage()
            //{
            //    ToAddress = pOrder.Customer.Email,
            //    Message = "There was an error in processsing your order " + pOrder.OrderNumber + ": "+ pException.Message +". Please contact Video Store"
            //});
            Common.Model.SendEmailMessage emailMessage = new Common.Model.SendEmailMessage()
            {
                Topic = "Email",
                Message = "There was an error in processsing your order " + pOrder.OrderNumber + ": " + pException.Message + ". Please contact Video Store",
                ToAddresses = pOrder.Customer.Email,
                Date = new DateTime()
            };

            PublisherServiceClient lClient = new PublisherServiceClient();
            lClient.Publish(emailMessage);
        }

        private void SendOrderPlacedConfirmation(Order pOrder)
        {
            //EmailProvider.SendMessage(new EmailMessage()
            //{
            //    ToAddress = pOrder.Customer.Email,
            //    Message = "Your order " + pOrder.OrderNumber + " has been placed"
            //});
            Common.Model.SendEmailMessage emailMessage = new Common.Model.SendEmailMessage()
            {
                Topic = "Email",
                Message = "Your order " + pOrder.OrderNumber + " has been placed",
                ToAddresses = pOrder.Customer.Email,
                Date = new DateTime()
            };

            PublisherServiceClient lClient = new PublisherServiceClient();
            lClient.Publish(emailMessage);
        }

        private void PlaceDeliveryForOrder(Order pOrder)
        {
            Guid identifier = Guid.NewGuid();
            Delivery lDelivery = new Delivery() { DeliveryStatus = DeliveryStatus.Submitted, SourceAddress = "Video Store Address", DestinationAddress = pOrder.Customer.Address, Order = pOrder };

            //Guid lDeliveryIdentifier = 
            //ExternalServiceFactory.Instance.DeliveryService.SubmitDelivery(new DeliveryInfo()
            //{
            //    OrderNumber = lDelivery.Order.OrderNumber.ToString(),
            //    SourceAddress = lDelivery.SourceAddress,
            //    DestinationAddress = lDelivery.DestinationAddress,
            //    DeliveryNotificationAddress = "net.tcp://localhost:9010/DeliveryNotificationService"
            //});

            Common.Model.SubmitDeliveryMessage deliveryMessage = new Common.Model.SubmitDeliveryMessage()
            {
                Topic = "DeliveryCo",
                OrderNumber = lDelivery.Order.OrderNumber.ToString(),
                SourceAddress = lDelivery.SourceAddress,
                DestinationAddress = lDelivery.DestinationAddress,
                DeliveryNotificationAddress = "VideoStore" //Topic
            };
            PublisherServiceClient lClient = new PublisherServiceClient();
            lClient.Publish(deliveryMessage);

            lDelivery.ExternalDeliveryIdentifier = identifier;
            pOrder.Delivery = lDelivery;
            
        }

        private void TransferFundsFromCustomer(int pCustomerAccountNumber, double pTotal, Guid pOrderNumber)
        {
            try
            {
                //ExternalServiceFactory.Instance.TransferService.Transfer(pTotal, pCustomerAccountNumber, RetrieveVideoStoreAccountNumber());
                Common.Model.TransferMessage transferMessage= new Common.Model.TransferMessage() {
                    Topic="Bank",
                    pAmount = pTotal,
                    pExternalOrderNumber = pOrderNumber,
                    pFromAcctNumber = pCustomerAccountNumber,
                    pToAcctNumber = RetrieveVideoStoreAccountNumber()
                };
                PublisherServiceClient lClient = new PublisherServiceClient();
                lClient.Publish(transferMessage);
            }
            catch(Exception e)
            {
                throw new Exception("Error Transferring funds for order.");
            }
        }


        private int RetrieveVideoStoreAccountNumber()
        {
            return 123;
        }


    }
}
