using System.ComponentModel.DataAnnotations;

namespace TCGshopTestEnvironment.Models
{
    public class Pictures
    {
        [Key]
        public int Picture_ID { get; set; }

        public string Picture { get; set; }
    }
}