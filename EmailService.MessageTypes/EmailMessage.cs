using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.MessageTypes
{
    public class EmailMessage
    {
        public String ToAddresses { get; set; }
        public String FromAddresses { get; set; }
        public String CCAddresses { get; set; }
        public String BCCAddresses { get; set; }
        public DateTime Date { get; set; }
        public String Message { get; set; }
    }
}
