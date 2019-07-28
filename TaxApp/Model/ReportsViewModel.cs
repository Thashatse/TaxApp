using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ReportsViewModel
    {
        public Model.DashboardIncomeExpense DashboardIncomeExpense { get; set; }
        public TaxDashboard TAXDashboard { get; set; }
        public VATDashboard VATDashboard { get; set; }
    }
}
