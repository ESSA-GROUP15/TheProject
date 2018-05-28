using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using DeliveryCo.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.ServiceLocatorAdapter;
using Microsoft.Practices.ServiceLocation;
using System.Configuration;
using System.Messaging;
using Common;

namespace DeliveryCo.Process
{
    class Program
    {
        private const String cAddress = "net.msmq://localhost/private/DeliveryQueueTransacted";
        private const String cMexAddress = "net.tcp://localhost:9030/DeliveryQueueTransacted/mex";
        private const String queue_dir = ".\\private$\\DeliveryQueueTransacted";

        static void Main(string[] args)
        {
            ResolveDependencies();
            //using (ServiceHost lHost = new ServiceHost(typeof(DeliveryService)))
            //{
            //    lHost.Open();
            //    Console.WriteLine("Delivery Service started. Press Q to quit");
            //    while (Console.ReadKey().Key != ConsoleKey.Q) ;
            //}
            using (SubscriberServiceHost lHost = new SubscriberServiceHost(typeof(SubscriberService), cAddress, cMexAddress, true, queue_dir))
            {
                Console.WriteLine("Delivery Service started. Press Q to quit");
                while (Console.ReadKey().Key != ConsoleKey.Q) ;
                lHost.Dispose();
            }
        }

        private static void ResolveDependencies()
        {

            UnityContainer lContainer = new UnityContainer();
            UnityConfigurationSection lSection
                    = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            lSection.Containers["containerOne"].Configure(lContainer);
            UnityServiceLocator locator = new UnityServiceLocator(lContainer);
            ServiceLocator.SetLocatorProvider(() => locator);
        }



        private static void EnsureQueueExists()
        {
            string queueName = ConfigurationManager.AppSettings["QueueAddress"];

            // Create the transacted MSMQ queue if necessary.
            if (!MessageQueue.Exists(queueName))
                MessageQueue.Create(queueName, true);
        }
    }
}