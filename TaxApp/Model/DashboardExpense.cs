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
        public decimal amount { get; set; }
        public string deatilTitle { get; set; }
        public string deatil { get; set; }
        public string date { get; set; }
        public DateTime dateSort { get; set; }
        public string URL { get; set; }
    }
}
