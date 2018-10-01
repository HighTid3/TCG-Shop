using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCGshopTestEnvironment.Models
{
    public class Orders
    {
        [Key]
        public int Order_ID { get; set; }

        public string Shipping_Status { get; set; }

        public DateTime Purchage_Date { get; set; }

        public string Payment_Status { get; set; }

        public DateTime Shipped_Date { get; set; }   
   }
}
