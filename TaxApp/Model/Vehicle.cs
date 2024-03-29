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

    public partial class Vehicle
    {
        public int VehicleID { get; set; }
        public int ProfielID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "SARS Fixed Cost (R p.a.)")]
        public decimal SARSFixedCost { get; set; }
        [Required]
        [Display(Name = "SARS Fuel Cost (R/KM)")]
        public decimal SARSFuelCost { get; set; }
        [Required]
        [Display(Name = "SARS Maintence Cost (R/KM)")]
        public decimal SARSMaintenceCost { get; set; }
        [Required]
        [Display(Name = "Client Charge (R/KM)")]
        public decimal ClientCharge { get; set; } 
    }
}