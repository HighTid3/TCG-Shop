using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TCGshopTestEnvironment.Models
{
    public class Products
    {
        [Key]
        public int Product_ID { get; set; }

        public float Price { get; set; }

        public string Description { get; set; }

        public string Grade { get; set; }

        public int Stock { get; set; }

        public DateTime Date_Created { get; set; }

        public DateTime Date_Updated { get; set; }

        public int Views_Listed { get; set; }

        public string Views_Details { get; set; }

   }
}
