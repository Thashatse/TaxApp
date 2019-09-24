//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class SP_GetJobExpense_Result
    {
        public int ExpenseID { get; set; }

        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int JobID { get; set; }
        [Required]
        public System.DateTime Date { get; set; }
        public String DateString { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public string AmountString { get; set; }
        public byte[] Invoice_ReceiptCopy { get; set; }

        [Display(Name = "Category")]
        public string CatName { get; set; }

        [Display(Name = "Category Description")]
        public string CatDescription { get; set; }
        public string JobTitle { get; set; }
        public string DefultDate { get; set; }
        public string MinDate { get; set; }
        public string MaxDate { get; set; }
        public string dropDownID { get; set; }
        public bool invoiced { get; set; }
    }
}
