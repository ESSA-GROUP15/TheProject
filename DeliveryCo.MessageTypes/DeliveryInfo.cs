using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCo.MessageTypes
{
    public class DeliveryInfo
    {
        public String SourceAddress { get; set; }
        public String DestinationAddress { get; set;  }
        public String OrderNumber { get; set; }
        public Guid DeliveryIdentifier { get; set; }
        public String DeliveryNotificationAddress { get; set; }
        public Int32 Status { get; set; }
    }
}
