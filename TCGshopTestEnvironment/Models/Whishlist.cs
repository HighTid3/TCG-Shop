using System.ComponentModel.DataAnnotations;

namespace TCGshopTestEnvironment.Models
{
    public class Wishlist
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public UserAccount User { get; set; }
        public int ProductId { get; set; }
        public Products Product { get; set; }
    }
}