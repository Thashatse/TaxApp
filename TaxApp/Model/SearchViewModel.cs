using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SearchViewModel
    {
        public long ResultID { get; set; }
        public string ResultTitle { get; set; }
        public string ResultDetails { get; set; }
        public string ResultDateString { get; set; }
        public string ResultLink { get; set; }
        public DateTime ResultDate { get; set; }
    }
}
