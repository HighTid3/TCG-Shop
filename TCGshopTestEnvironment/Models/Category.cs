using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TCGshopTestEnvironment.Models.JoinTables;

namespace TCGshopTestEnvironment.Models
{
    public class Category
    {
        [Key]
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public virtual List<ProductCategory> Products { get; set; }
    }
}
