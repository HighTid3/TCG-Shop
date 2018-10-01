using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TCGshopTestEnvironment.Models
{
    public class UserAccount : IdentityUser
    {
        public string Country { get; set; }

        public string Address { get; set; }

        public string ZipCode { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }

        
    }
}
