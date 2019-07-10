using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class JobViewModel
    {
        public List<Model.SP_GetJob_Result> curentJobs { get; set; }
        public List<Model.SP_GetJob_Result> pastJobs { get; set; }
    }
}
