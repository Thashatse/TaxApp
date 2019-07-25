using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TaxPeriodRates
    {
        public int PeriodID { get; set; }
        public int RateID { get; set; }
        public decimal Rate { get; set; }
        public decimal Threashold { get; set; }
        public char Type { get; set; }
    }
}
