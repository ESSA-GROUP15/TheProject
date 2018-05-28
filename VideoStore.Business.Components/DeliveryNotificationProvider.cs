using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Business.Components.Interfaces;
using VideoStore.Business.Entities;
using Microsoft.Practices.ServiceLocation;
using System.Transactions;

namespace VideoStore.Business.Components
{
    public class DeliveryNotificationProvider : IDeliveryNotificationProvider
    {
        public IEmailProvider EmailProvider
        {
            get { return ServiceLocator.Current.GetInstance<IEmailProvider>(); }
        }

        public void NotifyDeliveryCompletion(Guid pDeliveryId, Entities.DeliveryStatus status)
        {
            Order lAffectedOrder = RetrieveDeliveryOrder(pDeliveryId);
            UpdateDeliveryStatus(pDeliveryId, status);
            if (status == Entities.DeliveryStatus.Delivered)
            {
                EmailProvider.SendMessage(new EmailMessage()
                {
                    ToAddress = lAffectedOrder.Customer.Email,
                    Message = "Our records show that your order" +lAffectedOrder.OrderNumber + " has been delivered. Thank you for shopping at video store"
                });
            }
            if (status == Entities.DeliveryStatus.Failed)
            {
                EmailProvider.SendMessage(new EmailMessage()
                {
                    ToAddress = lAffectedOrder.Customer.Email,
                    Message = "Our records show that there was a problem" + lAffectedOrder.OrderNumber + " delivering your order. Please contact Video Store"
                });
            }
        }
        public void NotifyDeliveryProcessed(string orderNnmber, Guid pDeliveryId, DeliveryStatus status, String errorMsg)
        {
            using (TransactionScope lScope = new TransactionScope())
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                var orderN = Guid.Parse(orderNnmber);
                Delivery lDelivery = lContainer.Deliveries.Include("Order.Customer").Where((pDel) => pDel.Order.OrderNumber == orderN).FirstOrDefault();
                if (lDelivery != null)
                {
                    lDelivery.DeliveryStatus = status;

                    if (status == DeliveryStatus.Submitted)
                    {
                        lDelivery.ExternalDeliveryIdentifier = pDeliveryId;

                        SendOrderPlacedConfirmation(lDelivery.Order);
                    }
                    else if (status == DeliveryStatus.Failed)
                    {
                        SendOrderErrorMessage(lDelivery.Order, errorMsg);
                    }

                    lContainer.SaveChanges();

                }
                lScope.Complete();
            }
        }

        private void SendOrderErrorMessage(Order pOrder, String errorMsg)
        {
            EmailProvider.SendMessage(new EmailMessage()
            {
                ToAddress = pOrder.Customer.Email,
                Message = "There was an error in processsing your order " + pOrder.OrderNumber + ": " + errorMsg + ". Please contact Video Store"
            });
        }

        private void SendOrderPlacedConfirmation(Order pOrder)
        {
            EmailProvider.SendMessage(new EmailMessage()
            {
                ToAddress = pOrder.Customer.Email,
                Message = "Your order " + pOrder.OrderNumber + " has been placed"
            });
        }

        private void UpdateDeliveryStatus(Guid pDeliveryId, DeliveryStatus status)
        {
            using (TransactionScope lScope = new TransactionScope())
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                Delivery lDelivery = lContainer.Deliveries.Where((pDel) => pDel.ExternalDeliveryIdentifier == pDeliveryId).FirstOrDefault();
                if (lDelivery != null)
                {
                    lDelivery.DeliveryStatus = status;
                    lContainer.SaveChanges();
                }
                lScope.Complete();
            }
        }

        private Order RetrieveDeliveryOrder(Guid pDeliveryId)
        {
 	        using(VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                Delivery lDelivery =  lContainer.Deliveries.Include("Order.Customer").Where((pDel) => pDel.ExternalDeliveryIdentifier == pDeliveryId).FirstOrDefault();
                return lDelivery.Order;
            }
        }
    }


}
