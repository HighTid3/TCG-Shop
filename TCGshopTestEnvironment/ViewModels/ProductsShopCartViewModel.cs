using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCGshopTestEnvironment.ViewModels
{
    public class ProductsShopCartViewModel
    {
        public int CartID { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }
        public string Grade { get; set; }
        public int Amount { get; set; }

        public double TotalPrice { get; set; } 
    }
    
}
