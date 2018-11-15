using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.Models.JoinTables;
using TCGshopTestEnvironment.Services;
using TCGshopTestEnvironment.ViewModels;

namespace TCGshopTestEnvironment.Controllers
{
    public class WishlistController : Controller
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;
        private DBModel _context;
        private IWishlist _assets;

        public WishlistController(UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager, DBModel context, IWishlist assets)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _assets = assets;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View();
            }
            else
            {
                var cartproducts = _assets.WishlistItems(user.Id).ToList();
                return View(cartproducts);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            var assetModel = _assets.WishlistByUserid(user.Id).ToList(); //gets the basket of the logged in user

            if (!assetModel.Select(x => x.ProductId).Contains(productId)) // the basket already contains the product, add the amount by 1
            {
                var wishlist = new Wishlist { UserId = user.Id, ProductId = productId};
                _context.wishlists.Add(wishlist);
                _context.SaveChanges();


            }


            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult RemoveFromWishlist(int id)
        {
            var wishlistitem = _context.wishlists.FirstOrDefault(x => x.Id == id);

            if (wishlistitem != null)
            {
                _context.wishlists.Remove(wishlistitem);
                _context.SaveChanges(); // Save changes
            }
            var results = new ShoppingCartRemoveViewModel
            {

                DeleteId = id

            };
            return Json(results);
        }
    }
}
