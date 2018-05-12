using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoStore.Services.MessageTypes;

namespace VideoStore.WebClient.ViewModels
{
    public class EditUserDetailsViewModel
    {

        public EditUserDetailsViewModel() { }

        public EditUserDetailsViewModel(User pUser)
        {
            Email = pUser.Email;
            Address = pUser.Address;
        }


        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email
        {
            get;
            set;
        }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Postal Address")]
        public string Address
        {
            get;
            set;
        }


        public User UpdateMessageType(User pUser)
        {
            pUser.Email = Email;
            pUser.Address = Address;
            return pUser;
        }
    }
}