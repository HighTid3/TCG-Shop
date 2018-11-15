using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TCGshopTestEnvironment.Models.JoinTables;

namespace TCGshopTestEnvironment.Models
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }

        public string Name { get; set; }

        public UserAccount Owner { get; set; }

        public float Price { get; set; }

        public string Description { get; set; }

        public string Grade { get; set; }

        public int Stock { get; set; }
        public DateTime DateCreated { get; set; }
        

        public DateTime DateUpdated { get; set; }

        public int ViewsListed { get; set; }

        public int ViewsDetails { get; set; }

        public string ImageUrl { get; set; }

        public virtual List<ProductCategory> Category { get; set; }

        public virtual List<Wishlist> User { get; set; }
    }
}
