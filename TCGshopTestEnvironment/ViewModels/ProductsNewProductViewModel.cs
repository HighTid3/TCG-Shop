using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCGshopTestEnvironment.ViewModels
{
    public class ProductsNewProductViewModel
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }
        public string Grade { get; set; }
        public int Stock { get; set; }
        public List<string> Category { get; set; }

    }
}


