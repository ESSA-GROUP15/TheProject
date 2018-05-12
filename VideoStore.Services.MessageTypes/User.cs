using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoStore.Services.MessageTypes
{
    public class User : MessageType,  IUser<int>, IUser<string>
    {
        public String Name { get; set; }
        public String Email { get; set; }
        public String Address { get; set; }
        public LoginCredential LoginCredential {get; set;}

        public byte[] Revision { get; set; }


        public string UserName
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        string IUser<string>.Id
        {
            get { return this.Id.ToString(); }
        }
    }
}
