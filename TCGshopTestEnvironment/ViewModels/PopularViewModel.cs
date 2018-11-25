using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCGshopTestEnvironment.ViewModels
{
    public class PopularViewModel
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public int ViewsListed { get; set; }

        public int ViewsDetails { get; set; }

        public string ImageUrl { get; set; }

    
    }
}
