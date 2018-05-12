using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Services
{
    public class ServiceFactory
    {
        public static T GetService<T>()
        {
            return ServiceLocator.Current.GetInstance<T>();
        }
    }
}
