using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCGshopTestEnvironment.Models.JoinTables;
using TCGshopTestEnvironment.ViewModels;

namespace TCGshopTestEnvironment.Services
{
    public interface IShopping
    {
        IQueryable<ShoppingBasket> ShoppingbasketByName(string name);

        IQueryable<ProductsShopCartViewModel> ShoppinCartItems(string name);
    }


}
