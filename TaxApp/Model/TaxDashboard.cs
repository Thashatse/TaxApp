using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TaxDashboard
    {
        public decimal Income { get; set; }
        public string IncomeSTRING { get; set; }
        public decimal IncomePercent { get; set; }
        public string IncomePercentString { get; set; }
        public char IncomeUporDown { get; set; }
        public decimal TAXOwed { get; set; }
        public string TAXOwedSTRING { get; set; }
        public decimal TAXOwedPercent { get; set; }
        public decimal TAXRate { get; set; }
        public string TAXOwedPercentString { get; set; }
        public char TAXOwedUporDown { get; set; }
        public string TaxBraketString { get; set; }
        public string PeriodString { get; set; }
    }
}
