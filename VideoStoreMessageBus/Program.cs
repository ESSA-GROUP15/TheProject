using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VideoStoreMessageBus
{
    class Program
    {
        private static readonly String sPublishQueuePath = ".\\private$\\PublisherMessageQueueTransacted";

        static void Main(string[] args)
        {
            EnsureMessageQueuesExists();

            //Manually added registry....
            SubscriptionRegistry subscription = new SubscriptionRegistry();

            using (ServiceHost lPubHost = new ServiceHost(typeof(PublisherService)))
            using (ServiceHost lSubHost = new ServiceHost(typeof(SubscriptionService)))
            {
                AttachMessageInspectorToHost(lPubHost);
                AttachMessageInspectorToHost(lSubHost);
                lPubHost.Open();
                lSubHost.Open();
                Console.WriteLine("Message Bus Started Press Q to quit");
                while (Console.ReadKey().Key != ConsoleKey.Q) ;
            }
        }

        private static void EnsureMessageQueuesExists()
        {
            // Create the transacted MSMQ queue if necessary.
            if (!MessageQueue.Exists(sPublishQueuePath))
                MessageQueue.Create(sPublishQueuePath, true);
        }

        private static void AttachMessageInspectorToHost(ServiceHost pHost)
        {
            ServiceMessageInspector lInspector = new ServiceMessageInspector();
            foreach (System.ServiceModel.Description.ServiceEndpoint lEndPoint in pHost.Description.Endpoints)
            {
                lEndPoint.Behaviors.Add(lInspector);
            }
        }
    }
}
