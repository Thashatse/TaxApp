using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DashboardIncomeExpense
    {
        public decimal IncomePast30Days { get; set; }
        public string IncomePast30DaysString { get; set; }
        public decimal IncomePast60to30DaysPercent { get; set; }
        public char IncomePast60to30DaysUporDown { get; set; }
        public decimal ExpensePast30Days { get; set; }
        public string ExpensePast30DaysString { get; set; }
        public decimal ExpensePast60to30DaysPercent { get; set; }
        public char ExpensePast60to30DaysUporDown { get; set; }
    }
}
