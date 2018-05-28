using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Model
{
    [DataContract]
    public class TransferResultMessage:Message
    {
        [DataMember]
        public Boolean Success;

        [DataMember]
        public string Message;
    }
}
