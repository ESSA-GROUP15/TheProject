using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Model
{
    [DataContract]
    public class SubmitDeliveryMessage:Message
    {
        [DataMember]
        public String OrderNumber;

        [DataMember]
        public String SourceAddress;

        [DataMember]
        public String DestinationAddress;

        [DataMember]
        public String DeliveryNotificationAddress;
    }
}
