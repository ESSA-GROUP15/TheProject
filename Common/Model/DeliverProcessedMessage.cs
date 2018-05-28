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
        string orderNnmber;

        [DataMember]
        Guid pDeliveryId;

        [DataMember]
        int status;

        [DataMember]
        String errorMsg;


    }
}
