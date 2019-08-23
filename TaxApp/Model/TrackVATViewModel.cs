using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TrackTAXandVATViewModel
    {
        public VATDashboard VATDashboard { get; set; }
        public TaxDashboard TAXDashboard { get; set; }
        public ReportViewModel Report { get; set; }
    }
}
