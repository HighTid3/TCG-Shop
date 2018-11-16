using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

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
