using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElainKlinikka2._0.Helpers
{
    internal class Payment
    {
        public int paymentID { get; set; }
        public string ownerID { get; set; }
        public string petID { get; set; }
        public string appointmentID { get; set; }
        public string paymentValue { get; set; }
        public string paid { get; set; }
    }
}
