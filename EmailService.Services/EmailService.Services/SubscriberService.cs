using Common.Model;
using EmailService.Business.Components.Interfaces;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmailService.Services
{
    public class SubscriberService : ISubscriberService
    {

        public IEmailProvider EmailProvider
        {
            get
            {
                return ServiceFactory.GetService<IEmailProvider>();
            }
        }

       

        public void PublishToSubscriber(Common.Model.Message pMessage)
        {
            if (pMessage is Common.Model.SendEmailMessage)
            {
                SendEmailMessage lMessage = pMessage as SendEmailMessage;
                EmailProvider.SendEmail(
                    MessageTypeConverter.Instance.Convert<
                    global::EmailService.MessageTypes.EmailMessage,
                    global::EmailService.Business.Entities.EmailMessage>(new MessageTypes.EmailMessage() {                       
                        Message = lMessage.Message,
                        ToAddresses = lMessage.ToAddresses,
                        Date = DateTime.Now
                    })
                    );
            }
        }
    }
}
