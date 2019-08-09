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
        public List<string> column1Data { get; set; }
        public List<string> column2Data { get; set; }
        public List<string> column3Data { get; set; }
        public List<string> column4Data { get; set; }
        public List<string> column5Data { get; set; }
        public List<string> column6Data { get; set; }
        public string column1Total { get; set; }
        public string column2Total { get; set; }
        public string column3Total { get; set; }
        public string column4Total { get; set; }
        public string column5Total { get; set; }
        public string column6Total { get; set; }
    }
}
