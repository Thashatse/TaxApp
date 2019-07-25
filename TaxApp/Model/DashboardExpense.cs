using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DashboardExpense
    {
        public string name { get; set; }
        public string amountTital { get; set; }
        public string expenseType { get; set; }
        public decimal amount { get; set; }
        public decimal VAT { get; set; }
        public string TotalString { get; set; }
        public string VATString { get; set; }
        public string deatilTitle { get; set; }
        public string deatil { get; set; }
        public string date { get; set; }
        public DateTime dateSort { get; set; }
        public string URL { get; set; }
    }
}
