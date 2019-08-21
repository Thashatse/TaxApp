using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TrackJobViewModel
    {
        public SP_GetJob_Result jobDetails { get; set; }
        public List<Worklog> Worklog { get; set; }
        public List<SP_GetJobExpense_Result> JobExpenses { get; set; }
        public List<TravelLog> JobTravelLog { get; set; }
        public List<SP_GetInvoice_Result> invoices { get; set; }

    }
}
