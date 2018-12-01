using System.ComponentModel.DataAnnotations;

namespace TCGshopTestEnvironment.ViewModels.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required] [EmailAddress] public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }

        [Required]
        [MaxLength(256)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(256)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public string Country { get; set; }

        public string Address { get; set; }

        [DataType(DataType.PostalCode)] public string ZipCode { get; set; }
    }
}