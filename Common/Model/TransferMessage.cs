using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Model
{
    [DataContract]
    public class TransferMessage:Message
    {
        [DataMember]
        public double pAmount;

        [DataMember]
        public int pFromAcctNumber;

        [DataMember]
        public int pToAcctNumber;
    }
}
