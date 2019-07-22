using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class incomeViewModel
    {
        public List<Model.SP_GetInvoice_Result> OutInvoices { get; set; }
        public List<Model.SP_GetInvoice_Result> PastInvoices { get; set; }
        public Model.DashboardIncome DashboardIncome { get; set; }
    }
}
