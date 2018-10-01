using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCGshopTestEnvironment.Models
{
    public class Pictures
    {
        [Key]
        public int Picture_ID { get; set; }

        public string Picture { get; set; }
   }
}
