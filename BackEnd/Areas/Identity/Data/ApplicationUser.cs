using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AccountApplication.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        //email
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        // status
        [PersonalData]
        public int Status { get; set; }

        //phone number
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string PhoneNumber { get; set; }

        //address
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Address { get; set; }
    }
}
