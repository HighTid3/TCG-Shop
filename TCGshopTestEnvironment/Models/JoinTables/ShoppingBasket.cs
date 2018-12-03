using System;
using System.ComponentModel.DataAnnotations;

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