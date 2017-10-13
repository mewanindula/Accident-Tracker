using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccidentTracker.Models
{
    public class DataViewModel
    {
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string uMobile { get; set; }
        public string uEmail { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string rMobile { get; set; }
        public string rEmail { get; set; }
        public string Password { get; set; }

        public string Status { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Force { get; set; }
        public string Flame { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}