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

    public partial class Business
    {
        public int BusinessID { get; set; }

        [Required]
        public decimal VATRate { get; set; }
        public string SMSSid { get; set; }
        public string SMSToken { get; set; }
    }
}