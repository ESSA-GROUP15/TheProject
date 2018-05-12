using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoStore.Services.MessageTypes;

namespace VideoStore.WebClient.ViewModels
{
    public class CheckOutViewModel
    {
        public CheckOutViewModel(User pUser)
        {
            UserName = pUser.UserName;
            Address = pUser.Address;
        }

        public String UserName { get; set; }

        public String Address { get; set; }
    }
}