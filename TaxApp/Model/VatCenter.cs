using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class VatCenter
    {
        public VATDashboard VATDashboard { get; set; }
        public List<VATRecivedList> VATRecivedList { get; set; }
        public List<Model.DashboardExpense> VATPaid { get; set; }
    }
}
