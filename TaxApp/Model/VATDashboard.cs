using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class VATDashboard
    {
        public decimal VATRECEIVED { get; set; }
        public decimal VATRECEIVEDPercent { get; set; }
        public string VATRECEIVEDPercentString { get; set; }
        public char VATRECEIVEDUporDown { get; set; }
        public string VATRECEIVEDString { get; set; }
        public decimal VATPAID { get; set; }
        public decimal VATPAIDPercent { get; set; }
        public string VATPAIDPercentString { get; set; }
        public char VATPAIDUporDown { get; set; }
        public string VATPAIDString { get; set; }
        public decimal VATPAIDOutstandingEst { get; set; }
        public char VATPAIDOutstandingEstUporDown { get; set; }
        public decimal VATPAIDOutstandingEstPercent { get; set; }
        public string VATPAIDOutstandingEstPercentString { get; set; }
        public string VATPAIDOutstandingEstString { get; set; }
        public string PeriodString { get; set; }
    }
}
