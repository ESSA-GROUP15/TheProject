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
    public class PublisherSynService : IPublisherSynService
    {
        public void PublishSyn(Message pMessage)
        {
            foreach (String lHandlerAddress in SubscriptionRegistry.Instance.GetTopicSubscribers(pMessage.Topic))
            {
                ISubscriberService lSubServ = ServiceFactory.GetService<ISubscriberService>(lHandlerAddress);
                lSubServ.PublishToSubscriber(pMessage);
            }
        }
    }
}
