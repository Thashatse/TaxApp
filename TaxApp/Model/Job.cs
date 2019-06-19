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
        public int ClientID { get; set; }

        [Required]
        public string JobTitle { get; set; }


        public Nullable<decimal> HourlyRate { get; set; }
        public Nullable<decimal> Budget { get; set; }

        [Required]
        public System.DateTime StartDate { get; set; }

        public Nullable<System.DateTime> EndDate { get; set; }
    }
}
