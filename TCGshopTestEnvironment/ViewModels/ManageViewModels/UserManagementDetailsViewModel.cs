using System.ComponentModel.DataAnnotations;

namespace TCGshopTestEnvironment.ViewModels.ManageViewModels
{
    public class UserManagementDetailsViewModel
    {
        [Required]
        [MaxLength(256)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "The Email Address already exists")]
        [RegularExpression(
            @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$",
            ErrorMessage = "Invalid email format.")]
        [Required(ErrorMessage = "Please enter your e-mail address.")]
        [StringLength(50)]
        [MaxLength(256)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        [DataType(DataType.PhoneNumber)] public string PhoneNumber { get; set; }

        public string Country { get; set; }

        public string Address { get; set; }

        [DataType(DataType.PostalCode)] public string ZipCode { get; set; }

        [MaxLength(256)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [MaxLength(256)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
    }
}