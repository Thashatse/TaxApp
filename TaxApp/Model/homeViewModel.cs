using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class homeViewModel
    {
        public List<Model.SP_GetJob_Result> Jobs { get; set; }
        public List<Model.DashboardExpense> Expenses { get; set; }
        public List<Model.SP_GetInvoice_Result> Invoices { get; set; }
        public Model.DashboardIncomeExpense DashboardIncomeExpense { get; set; }
        public List<Model.SP_GetInvoice_Result> OutInvoices { get; set; }
        //public Model.Vat Vat { get; set; }
    }
}
