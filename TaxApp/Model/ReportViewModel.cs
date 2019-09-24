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
        public string reportSubHeading { get; set; }
        public string reportCondition { get; set; }
        public string reportStartDate{ get; set; }
        public string reportEndDate{ get; set; }

        public string chartData { get; set; }
        public string chartLabels { get; set; }

        public string column1Name { get; set; }
        public string column2Name { get; set; }
        public string column3Name { get; set; }
        public string column4Name { get; set; }
        public string column5Name { get; set; }
        public string column6Name { get; set; }
        public string column7Name { get; set; }
        public string column8Name { get; set; }
        public string column9Name { get; set; }
        public string column10Name { get; set; }

        public List<ReportDataList> ReportDataList { get; set; }

        public List<ReportFixedFooterRowList> FooterRowList { get; set; }

        public string column1Total { get; set; }
        public string column2Total { get; set; }
        public string column3Total { get; set; }
        public string column4Total { get; set; }
        public string column5Total { get; set; }
        public string column6Total { get; set; }
        public string column7Total { get; set; }
        public string column8Total { get; set; }
        public string column9Total { get; set; }
        public string column10Total { get; set; }

        public bool column1DataAlignRight { get; set; } = false;
        public bool column2DataAlignRight { get; set; } = false;
        public bool column3DataAlignRight { get; set; } = false;
        public bool column4DataAlignRight { get; set; } = false;
        public bool column5DataAlignRight { get; set; } = false;
        public bool column6DataAlignRight { get; set; } = false;
        public bool column7DataAlignRight { get; set; } = false;
        public bool column8DataAlignRight { get; set; } = false;
        public bool column9DataAlignRight { get; set; } = false;
        public bool column10DataAlignRight { get; set; } = false;
        public bool column1FotterAlignRight { get; set; } = false;
        public bool column2FotterAlignRight { get; set; } = false;
        public bool column3FotterAlignRight { get; set; } = false;
        public bool column4FotterAlignRight { get; set; } = false;
        public bool column5FotterAlignRight { get; set; } = false;
        public bool column6FotterAlignRight { get; set; } = false;
        public bool column7FotterAlignRight { get; set; } = false;
        public bool column8FotterAlignRight { get; set; } = false;
        public bool column9FotterAlignRight { get; set; } = false;
        public bool column10FotterAlignRight { get; set; } = false;
    }
}
