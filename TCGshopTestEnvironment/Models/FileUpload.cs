using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TCGshopTestEnvironment.Models
{
    public class FileUpload
    {
        [Required]
        [Display(Name = "Public Schedule")]
        public IFormFile UploadPublicSchedule { get; set; }
    }
}