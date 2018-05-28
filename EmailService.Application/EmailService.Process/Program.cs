using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.ServiceLocatorAdapter;
using Microsoft.Practices.ServiceLocation;
using System.Configuration;
using Common;
using EmailService.Services;

namespace EmailService.Process
{
    class Program
    {
        private const String cAddress = "net.msmq://localhost/private/EmailServiceQueueTransacted";
        private const String cMexAddress = "net.tcp://localhost:9028/EmailServiceQueueTransacted/mex";
        private const String queue_dir = ".\\private$\\EmailServiceQueueTransacted";

        static void Main(string[] args)
        {
            ResolveDependencies();
            //using (ServiceHost lHost = new ServiceHost(typeof(EmailService.Services.EmailService)))
            //{
            //    lHost.Open();
            //    Console.WriteLine("Email Service Started");
            //    while (Console.ReadKey().Key != ConsoleKey.Q) ;
            //}
            using (SubscriberServiceHost lHost = new SubscriberServiceHost(typeof(SubscriberService), cAddress, cMexAddress, true, queue_dir))
            {
                Console.WriteLine("Email Service started. Press Q to quit");
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
    }
}
