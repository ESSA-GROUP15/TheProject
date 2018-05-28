using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Model
{
    [DataContract]
    public class DeliverCompleteMessage:Message
    {
        [DataMember]
        Guid pDeliveryId;

        [DataMember]
        int status;
    }
}
