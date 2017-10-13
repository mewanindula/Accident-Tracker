using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AccidentTracker.Models
{
    [MetadataType(typeof(UserMetaData))]

    public partial class User
    {
        public string ConfirmPassword { get; set; }
    }

    public class UserMetaData
    {

        [Display(Name="First Name")]
        [Required(AllowEmptyStrings=false, ErrorMessage="First Name Required")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name Required")]
        public string LastName { get; set; }


        [Display(Name = "User Mobile Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "User Mobile Number Required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string uMobile { get; set; }


        [Display(Name = "User Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "User Email ID Required")]
        [DataType(DataType.EmailAddress)]
        public string uEmail { get; set; }


        [Display(Name = "Date Of Birth")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Of Birth Required")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode=true, DataFormatString="{0:MM/dd/yyyy}")]
        public string DateOfBirth { get; set; }


        [Display(Name = "Relation Mobile Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Relation Mobile Number Required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string rMobile { get; set; }


        [Display(Name = "Relation Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Relation Email ID Required")]
        [DataType(DataType.EmailAddress)]
        public string rEmail { get; set; }


        [Display(Name = "User ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "User ID Required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string UserID { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage="Minimum 6 Characters Required")]
        public string Password { get; set; }


        [Display(Name="Confirm Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="Confirm Password And Password Do Not Match")]
        public string ConfirmPassword { get; set; }
    }
}