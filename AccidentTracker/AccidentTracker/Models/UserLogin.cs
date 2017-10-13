using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccidentTracker.Models
{
    public class UserLogin
    {

        [Display(Name="User ID")]
        [Required(AllowEmptyStrings=false, ErrorMessage="User ID Required")]
        public string UserID { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}