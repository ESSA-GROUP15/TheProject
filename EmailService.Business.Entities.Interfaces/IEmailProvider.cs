using EmailService.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Business.Components.Interfaces
{
    public interface IEmailProvider
    {
        void SendEmail(EmailMessage pMessage);
    }
}
