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
    using System.Web.Security;

    public partial class Profile
    {
        public int ProfileID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        ErrorMessage = "Sorry, The email address entered is not in the correct format. The standard email address format is name@example.com")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile No")]
        [Display(Name = "Contact No.")]
        [StringLength(10, ErrorMessage = "Please enter a valid phone number", MinimumLength = 10)]
        public string ContactNumber { get; set; }

        [Display(Name = "Address")]
        public string PhysicalAddress { get; set; }


        public byte[] ProfilePicture { get; set; }


        [Display(Name = "VAT Number")]
        public string VATNumber { get; set; }

        [Display(Name = "Current VAT Rate")]
        public decimal VATRate { get; set; }

        [Required]
        [Display(Name = "Default Hourly Rate (R)")]
        public decimal DefaultHourlyRate { get; set; }


        public bool Active { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [MembershipPassword(
            MinRequiredNonAlphanumericCharacters = 1,
            MinNonAlphanumericCharactersError = "Your password needs to contain at least one symbol (!, @, #, etc).",
            ErrorMessage = "Your password must be 6 characters long and contain at least one symbol (!, @, #, etc).",
            MinRequiredPasswordLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match, please try again!")]
        [Display(Name = "Confirm password")]
        public string PasswordConfirmation { get; set; }

        [Compare("NewPassword", ErrorMessage = "Passwords do not match, please try again!")]
        [Display(Name = "Confirm New password")]
        public string NewPasswordConfirmation { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }
        public string PassRestCode { get; set; }
    }
} 