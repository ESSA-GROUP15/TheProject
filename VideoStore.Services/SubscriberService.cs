using Common;
using DeliveryCo.Services.Interfaces;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoStore.Business.Components.Interfaces;
using VideoStore.Business.Entities;

namespace VideoStore.Services
{
    public class SubscriberService : ISubscriberService
    {

        IDeliveryNotificationProvider NotificationProvider
        {
            get { return ServiceLocator.Current.GetInstance<IDeliveryNotificationProvider>(); }
        }

        private IOrderProvider OrderProvider
        {
            get
            {
                return ServiceFactory.GetService<IOrderProvider>();
            }
        }



        public void PublishToSubscriber(Common.Model.Message pMessage)
        {
            if (pMessage is Common.Model.DeliverProcessedMessage)
            {
                Common.Model.DeliverProcessedMessage lMessage = pMessage as Common.Model.DeliverProcessedMessage;
                string orderNumber = lMessage.orderNumber;
                Guid pDeliveryId = lMessage.pDeliveryId;
                DeliveryStatus status = (DeliveryStatus)lMessage.status;
                string errorMsg = lMessage.errorMsg;
                NotificationProvider.NotifyDeliveryProcessed(orderNumber, pDeliveryId, status, errorMsg);
            }

            else if (pMessage is Common.Model.DeliverCompleteMessage)
            {
                Common.Model.DeliverCompleteMessage lMessage = pMessage as Common.Model.DeliverCompleteMessage;
                Guid pDeliveryId = lMessage.pDeliveryId;
                DeliveryStatus status = (DeliveryStatus)lMessage.status;
                NotificationProvider.NotifyDeliveryCompletion(pDeliveryId, status);
            }

            else if (pMessage is Common.Model.TransferResultMessage)
            {
                Common.Model.TransferResultMessage lMessage = pMessage as Common.Model.TransferResultMessage;
                Boolean success = lMessage.Success;
                String Msg = lMessage.Message;
                Guid orderId = lMessage.OrderNumber;
               
                OrderProvider.AfterTransferResultReturns(success, orderId, Msg);
               
            }

           
        }
    }
}
