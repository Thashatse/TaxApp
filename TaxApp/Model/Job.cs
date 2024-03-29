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

    public partial class Job
    {
        public int JobID { get; set; }

        [Required]
        [Display(Name = "Client")]
        public int ClientID { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string JobTitle { get; set; }

        [Required]
        [Display(Name = "Hourly Rate (R)")]
        public Nullable<decimal> HourlyRate { get; set; }

        [Display(Name = "Budget (R)")]
        public Nullable<decimal> Budget { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public System.DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public Nullable<System.DateTime> EndDate { get; set; }
        public string DefultDate { get; set; }
        public string MinDate { get; set; }

        [Required]
        [Display(Name = "Share this job with the Client. Clients will be able to see hours, expenses and invoices.")]
        public bool Share { get; set; }
    }
} 