using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using EmailService.MessageTypes;

namespace EmailService.Services.Interfaces
{
    [ServiceContract]
    public interface IEmailService
    {
        [OperationContract]
        void SendEmail(EmailMessage pMessage);
    }
}
