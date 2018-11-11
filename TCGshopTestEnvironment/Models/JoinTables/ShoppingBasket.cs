using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCGshopTestEnvironment.Models.JoinTables
{
    public class ShoppingBasket
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public int ProductsId { get; set; }

        public int Amount { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
