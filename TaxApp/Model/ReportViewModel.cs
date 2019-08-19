using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ReportViewModel
    {
        public string reportTitle { get; set; }
        public string reportCondition { get; set; }
        public string reportStartDate{ get; set; }
        public string reportEndDate{ get; set; }
        public string column1Name { get; set; }
        public string column2Name { get; set; }
        public string column3Name { get; set; }
        public string column4Name { get; set; }
        public string column5Name { get; set; }
        public string column6Name { get; set; }
        public string column7Name { get; set; }
        public List<ReportDataList> ReportDataList { get; set; }
        public List<ReportFixedFooterRowList> FooterRowList { get; set; }
        public string column1Total { get; set; }
        public string column2Total { get; set; }
        public string column3Total { get; set; }
        public string column4Total { get; set; }
        public string column5Total { get; set; }
        public string column6Total { get; set; }
        public string column7Total { get; set; }
    }
}
