using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TCGshopTestEnvironment.Models
{
    public class FileUpload
    {
        [Required]
        public IFormFile CardImageUpload { get; set; }
    }
}