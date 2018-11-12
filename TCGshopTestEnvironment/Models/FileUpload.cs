using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TCGshopTestEnvironment.Models
{
    public class FileUpload
    {
        [Required]
        public IFormFile CardImageUpload { get; set; }
    }
}