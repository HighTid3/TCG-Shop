using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCGshopTestEnvironment.Models
{
    public class Statistics
    {
        [Key]
        public int Static_ID { get; set; }
        public DateTime Date { get; set; }

   }
}
