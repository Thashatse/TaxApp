using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TAXorVATRecivedList
    {
        public int JobID { get; set; }
        public string JobTitle { get; set; }
        public string clientName { get; set; }
        public int clientID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceDateString { get; set; }
        public decimal Total { get; set; }
        public decimal VATorTAX { get; set; }
        public string TotalString { get; set; }
        public string VATorTAXString { get; set; }
    }
}
