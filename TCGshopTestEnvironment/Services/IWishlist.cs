using System.Linq;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.ViewModels;

namespace TCGshopTestEnvironment.Services
{
    public interface IWishlist
    {
        IQueryable<Wishlist> WishlistByUserid(string productId);

        IQueryable<WishlistViewModel> WishlistItems(string userid);
    }
}
