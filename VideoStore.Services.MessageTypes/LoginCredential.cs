using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoStore.Services.MessageTypes
{
    public class LoginCredential : MessageType
    {
        public String UserName { get; set; }
        public String EncryptedPassword { get; set; }
    }
}
