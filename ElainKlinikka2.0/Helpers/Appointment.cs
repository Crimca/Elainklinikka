using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElainKlinikka2._0.Helpers
{
    internal class Appointment
    {
        public string appointmentID { get; set; }
        public string Reason { get; set; }
        public string petID { get; set; }
        public string employeeID { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
    }
}
