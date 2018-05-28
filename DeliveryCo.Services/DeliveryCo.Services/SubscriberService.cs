using Common;
using Common.Model;
using DeliveryCo.Business.Components.Interfaces;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCo.Services
{
    public class SubscriberService : ISubscriberService
    {
        private IDeliveryProvider DeliveryProvider
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IDeliveryProvider>();
            }
        }


        public void PublishToSubscriber(Common.Model.Message pMessage)
        {
            if (pMessage is SubmitDeliveryMessage)
            {
                SubmitDeliveryMessage lMessage = pMessage as SubmitDeliveryMessage;
                DeliveryProvider.SubmitDelivery(
               MessageTypeConverter.Instance.Convert<DeliveryCo.MessageTypes.DeliveryInfo,
               DeliveryCo.Business.Entities.DeliveryInfo>(new MessageTypes.DeliveryInfo() {
                   OrderNumber = lMessage.OrderNumber,
                   SourceAddress = lMessage.SourceAddress,
                   DestinationAddress = lMessage.DestinationAddress,
                   DeliveryNotificationAddress = lMessage.DeliveryNotificationAddress
               })
           );
            }
        }
    }
}
