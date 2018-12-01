using System.Collections.Generic;

namespace TCGshopTestEnvironment.ViewModels
{
    public class AccountAndAddressViewModel
    {
        public List<OrderViewModel> OrderViewModel { set; get; }
        public List<ProductsShopCartViewModel> OrderDetails { set; get; }
    }
}