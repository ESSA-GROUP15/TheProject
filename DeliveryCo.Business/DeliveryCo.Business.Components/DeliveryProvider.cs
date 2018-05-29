using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeliveryCo.Business.Components.Interfaces;
using System.Transactions;
using DeliveryCo.Business.Entities;
using System.Threading;
using DeliveryCo.Services.Interfaces;
using DeliveryCo.Business.Components.PublisherService;

namespace DeliveryCo.Business.Components
{
    public class DeliveryProvider : IDeliveryProvider
    {
        public void SubmitDelivery(DeliveryCo.Business.Entities.DeliveryInfo pDeliveryInfo)
        {
            try
            {
                using (TransactionScope lScope = new TransactionScope())
                using (DeliveryDataModelContainer lContainer = new DeliveryDataModelContainer())
                {
                    pDeliveryInfo.DeliveryIdentifier = Guid.NewGuid();
                    pDeliveryInfo.Status = 0;
                    lContainer.DeliveryInfoes.AddObject(pDeliveryInfo);
                    lContainer.SaveChanges();
                    ThreadPool.QueueUserWorkItem(new WaitCallback((pObj) => ScheduleDelivery(pDeliveryInfo)));
                    //IDeliveryNotificationService lService = DeliveryNotificationServiceFactory.GetDeliveryNotificationService(pDeliveryInfo.DeliveryNotificationAddress);
                    //lService.NotifyDeliveryProcessed(pDeliveryInfo.OrderNumber, pDeliveryInfo.DeliveryIdentifier, DeliveryInfoStatus.Submitted, "");
                    Common.Model.DeliverProcessedMessage deliverProcessedMessage = new Common.Model.DeliverProcessedMessage()
                    {
                        Topic="VideoStore",
                        orderNumber = pDeliveryInfo.OrderNumber,
                        pDeliveryId = pDeliveryInfo.DeliveryIdentifier,
                        status = (int)DeliveryInfoStatus.Submitted,
                        errorMsg = ""
                    };
                    PublisherServiceClient lClient = new PublisherServiceClient();
                    lClient.Publish(deliverProcessedMessage);
                    lScope.Complete();
                }
                //  return pDeliveryInfo.DeliveryIdentifier;
            }
            catch (Exception ex)
            {
                using (TransactionScope lScope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    //IDeliveryNotificationService lService = DeliveryNotificationServiceFactory.GetDeliveryNotificationService(pDeliveryInfo.DeliveryNotificationAddress);
                    //lService.NotifyDeliveryProcessed(pDeliveryInfo.OrderNumber, pDeliveryInfo.DeliveryIdentifier, DeliveryInfoStatus.Failed, ex.ToString());

                    //lScope.Complete();
                    Common.Model.DeliverProcessedMessage deliverProcessedMessage = new Common.Model.DeliverProcessedMessage()
                    {
                        Topic="VideoStore",
                        orderNumber = pDeliveryInfo.OrderNumber,
                        pDeliveryId = pDeliveryInfo.DeliveryIdentifier,
                        status = (int)DeliveryInfoStatus.Failed,
                        errorMsg = ex.ToString()
                    };
                    PublisherServiceClient lClient = new PublisherServiceClient();
                    lClient.Publish(deliverProcessedMessage);
                    lScope.Complete();
                }

            }
            }

        private void ScheduleDelivery(DeliveryInfo pDeliveryInfo)
        {
            Console.WriteLine("Delivering to" + pDeliveryInfo.DestinationAddress);
            Thread.Sleep(3000);
            //notifying of delivery completion
            using (TransactionScope lScope = new TransactionScope())
            using (DeliveryDataModelContainer lContainer = new DeliveryDataModelContainer())
            {
                pDeliveryInfo.Status = 1;
                //IDeliveryNotificationService lService = DeliveryNotificationServiceFactory.GetDeliveryNotificationService(pDeliveryInfo.DeliveryNotificationAddress);
                //lService.NotifyDeliveryCompletion(pDeliveryInfo.DeliveryIdentifier, DeliveryInfoStatus.Delivered);
                Common.Model.DeliverCompleteMessage deliverCompleteMessage = new Common.Model.DeliverCompleteMessage()
                {
                    Topic="VideoStore",
                    pDeliveryId=pDeliveryInfo.DeliveryIdentifier,
                    status=(int)DeliveryInfoStatus.Delivered
                };
                PublisherServiceClient lClient = new PublisherServiceClient();
                lClient.Publish(deliverCompleteMessage);
                lScope.Complete();
            }

        }
    }
}
