using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TaxCenter
    {
        public TaxDashboard TAXDashboard { get; set; }
        public List<TAXorVATRecivedList> TAXRecivedList { get; set; }
        public TaxAndVatPeriods period { get; set; }
    }
}
