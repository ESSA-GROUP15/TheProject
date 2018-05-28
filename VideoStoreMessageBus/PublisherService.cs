using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using VideoStoreMessageBus.Interfaces;

namespace VideoStoreMessageBus
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class PublisherService : IPublisherService
    {
        public void Publish(Message pMessage)
        {
            foreach (String lHandlerAddress in SubscriptionRegistry.Instance.GetTopicSubscribers(pMessage.Topic))
            {
                ISubscriberService lSubServ = ServiceFactory.GetService<ISubscriberService>(lHandlerAddress);
                lSubServ.PublishToSubscriber(pMessage);
            }
        }
    }
}
