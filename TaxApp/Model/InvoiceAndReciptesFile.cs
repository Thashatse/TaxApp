using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Model
{
    public class InvoiceAndReciptesFile
    {
        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Select File:")]
        public HttpPostedFileBase file { get; set; }
        public byte[] fileByteArray { get; set; }
        public int ID { get; set; }
        public int ProfileID { get; set; }
        [Required]
        public string fileName { get; set; }
        [Required]
        public string expenseName { get; set; }
        [Required]
        public DateTime expenseDate { get; set; }
        public string expenseDateString { get; set; }
        public string Type { get; set; }
    }
}
