using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ExpenseViewModel
    {
        public List<Model.SP_GetGeneralExpense_Result> GExpense { get; set; }
        public List<Model.SP_GetJobExpense_Result> JExpense { get; set; }
        public List<Model.TravelLog> TExpense { get; set; }
    }
}
