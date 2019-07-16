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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class TravelLog
    {
        public int ExpenseID { get; set; }
        public string From { get; set; }
        public string FromGoogleURL { get; set; }
        public string To { get; set; }
        public string ToGoogleURL { get; set; }
        public string Reason { get; set; }
        public string JobTitle { get; set; }
        public string VehicleName { get; set; }
        public double OpeningKMs { get; set; }
        public Nullable<double> ClosingKMs { get; set; }
        public Nullable<double> TotalKMs { get; set; }
        [Display(Name = "Vehicle")]
        public int VehicleID { get; set; }
        public bool Invoiced { get; set; }
        public DateTime Date { get; set; }
        public String DateString { get; set; }
        public Nullable<int> JobID { get; set; }
        [Display(Name = "SARS Fuel Cost")]
        public decimal SARSFuelCost { get; set; }
        [Display(Name = "SARS Maintence Cost")]
        public decimal SARSMaintenceCost { get; set; }
        [Display(Name = "Client Charge")]
        public decimal ClientCharge { get; set; }
    }
}
