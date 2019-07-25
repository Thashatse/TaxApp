using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TAXRecivedList
    {
        public int JobID { get; set; }
        public string JobTitle { get; set; }
        public string clientName { get; set; }
        public int clientID { get; set; }
        public DateTime JobStartDate { get; set; }
        public string JobStartDateString { get; set; }
        public decimal Total { get; set; }
        public decimal TAX { get; set; }
        public string TotalString { get; set; }
        public string TAXString { get; set; }
    }
}
