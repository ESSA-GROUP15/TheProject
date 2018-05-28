using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoStoreMessageBus.Interfaces;

namespace VideoStoreMessageBus
{
    public class SubscriptionService : ISubscriptionService
    {
        public void Subscribe(string pTopic, string pCaller)
        {
            Console.WriteLine("Subscription for " + pTopic + " received");
            SubscriptionRegistry.Instance.AddSubscription(pTopic, pCaller);
        }

        public void Unsubscribe(string pTopic, string pCaller)
        {
            SubscriptionRegistry.Instance.RemoveSubscription(pTopic, pCaller);
        }
    }
}
