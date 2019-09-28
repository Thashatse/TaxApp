using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ConsultantViewModel
    {
        public TaxConsultant Consultant { get; set; }
        public List<TaxAndVatPeriods> taxPeriod { get; set; }
        public List<TaxAndVatPeriods> vatPeriod { get; set; }
    }
}
