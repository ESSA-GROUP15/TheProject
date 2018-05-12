using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoStore.Services.MessageTypes
{
    public class OrderItem : MessageType
    {
        public Media Media { get; set; }
        public int Quantity { get; set; }
    }
}
