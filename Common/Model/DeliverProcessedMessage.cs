using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Model
{
    [DataContract]
    public class DeliverProcessedMessage:Message
    {
        [DataMember]
        public String orderNumber;

        [DataMember]
        public Guid pDeliveryId;

        [DataMember]
        public int status;

        [DataMember]
        public String errorMsg;


    }
}
