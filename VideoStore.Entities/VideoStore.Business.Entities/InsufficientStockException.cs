using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoStore.Business.Entities
{
    public class InsufficientStockException : Exception
    {
        public String ItemName { get; set; }
    }
}
