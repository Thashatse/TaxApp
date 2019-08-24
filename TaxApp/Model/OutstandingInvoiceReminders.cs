using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class OutstandingInvoiceReminders
    {
        public string InvoiceNum { get; set; }
        public DateTime DateTime { get; set; }
        public string JobTitle { get; set; }
        public string ClientName { get; set; }
        public int ProfileID { get; set; }
        public int DaysSince { get; set; }
    }
}
