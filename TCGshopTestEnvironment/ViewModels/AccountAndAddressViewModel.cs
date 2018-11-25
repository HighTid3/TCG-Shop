using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCGshopTestEnvironment.ViewModels
{
    public class AccountAndAddressViewModel
    {
        public List<OrderViewModel> OrderViewModel { set; get; }
        public List<ProductsShopCartViewModel> OrderDetails { set; get; }
    }
}
