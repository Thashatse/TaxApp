using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TaxAndVatPeriods
    {
        public int ProfileID { get; set; }
        public int PeriodID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PeriodString { get; set; }
        public char Type { get; set; }
    }
}
