using System.Linq;
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