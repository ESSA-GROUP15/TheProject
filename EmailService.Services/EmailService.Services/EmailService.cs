using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmailService.Services.Interfaces;
using EmailService.Business.Components.Interfaces;
using EmailService.MessageTypes;
using Microsoft.Practices.ServiceLocation;

namespace EmailService.Services
{
    public class EmailService : IEmailService
    {
        public IEmailProvider EmailProvider
        {
            get
            {
                return ServiceFactory.GetService<IEmailProvider>();
            }
        }

        public void SendEmail(EmailMessage pMessage)
        {
            EmailProvider.SendEmail(
                    MessageTypeConverter.Instance.Convert<
                    global::EmailService.MessageTypes.EmailMessage,
                    global::EmailService.Business.Entities.EmailMessage>(pMessage)
                
                );
        }
    }
}
