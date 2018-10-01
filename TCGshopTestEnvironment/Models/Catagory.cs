using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TCGshopTestEnvironment.Models
{
    public class Catagory
    {
        [Key]
        public int Catagory_ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

   }
}
