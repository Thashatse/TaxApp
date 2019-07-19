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
    using DocumentFormat.OpenXml.Wordprocessing;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class SP_GetInvoice_Result
    {

        [Display(Name = "Invoice Number:")]
        public string InvoiceNum { get; set; }
        [Display(Name = "Date & Time")]
        public System.DateTime DateTime { get; set; }

        [Display(Name = "VAT Rate")]
        public decimal VATRate { get; set; }
        public String VATRateString { get; set; }
        public bool Paid { get; set; }
        public int LineItemID { get; set; }
        public string Name { get; set; }
        [Display(Name = "Unit Count")]
        public decimal UnitCount { get; set; }
        public string UnitCountString { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }
        public string UnitCostString { get; set; }

        [Display(Name = "Total Cost")]
        public decimal TotalCost { get; set; }
        public String TotalCostString { get; set; }
        public int JobID { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }
        public int ClientID { get; set; }

        [Display(Name = "Client")]
        public string ClientName { get; set; }

        [Display(Name = "Company")]
        public string CompanyName { get; set; }
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Physical Address")]
        public string PhysiclaAddress { get; set; }
        public char Type { get; set; }
    }
}