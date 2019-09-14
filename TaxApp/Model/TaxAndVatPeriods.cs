
namespace Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TaxAndVatPeriods
    {
        public int ProfileID { get; set; }
        public int PeriodID { get; set; }
        [Required]
        public decimal VATRate { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateString { get; set; }
        public DateTime EndDate { get; set; }
        public string EndDateString { get; set; }
        public string PeriodString { get; set; }
        public char Type { get; set; }
        
        [Required]
        [Display(Name = "Share this tax period with your Tax Consultant. Consultants will be able to see income, expenses, TAX, Vat, invoices and receipts")]
        public bool Share { get; set; }
    }
}
