using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCGshopTestEnvironment.ViewModels
{
    public class RegisterViewModel
    {
        [Required, MaxLength(256), Display(Name = "Username")]
        public string UserName { get; set; }
        [Required, EmailAddress, MaxLength(256), Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required, MinLength(6), MaxLength(50), DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }
        [Required, MaxLength(256), Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required, MaxLength(256), Display(Name = "Last name")]
        public string LastName { get; set; }

        public string Country { get; set; }

        public string Address { get; set; }
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required, MinLength(6), MaxLength(50), DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password does not match the confirmation password.")]
        public string ConfirmPassword { get; set; }
    }
}
