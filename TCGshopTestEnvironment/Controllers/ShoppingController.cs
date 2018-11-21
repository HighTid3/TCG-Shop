using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.Models.JoinTables;
using TCGshopTestEnvironment.Services;
using TCGshopTestEnvironment.ViewModels;

namespace TCGshopTestEnvironment.Controllers
{
    public class ShoppingController : Controller
    {
        public const string SessionKeyName = "_Cart";
        public const string TotalCartProducts = "_count";
        private DBModel _context;
        private IShopping _assets;
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;

        public ShoppingController(DBModel context, IShopping assets, UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager)
        {
            _context = context;
            _assets = assets;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // get the shopping cart items from database and return it to view
        [HttpGet]
        public async Task<IActionResult> ShoppingCart()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View();
            }
            else
            {
                var cartproducts = _assets.ShoppinCartItems(user.Id).ToList();
                return View(cartproducts);
            }
        }

        //add items to the shopping cart
        [HttpPost]
        public async Task<IActionResult> AddToShoppingcart(string userName, int productId, int Amount)
        {
            var user = await _userManager.GetUserAsync(User);
            if (userName != user.UserName)
            {
                return Json(new { error = "userId did not match with the session" });
            }
            var assetModel = _assets.ShoppingbasketByName(user.Id).ToList(); //gets the basket of the logged in user

            if (assetModel.Select(x => x.ProductsId).Contains(productId)) // the basket already contains the product, add the amount by 1
            {

                ShoppingBasket updatedmodel = assetModel.FirstOrDefault(x => x.ProductsId == productId);
                updatedmodel.Amount += Amount;
                _context.Update(updatedmodel);
                _context.SaveChanges();

            }

            else // else add the product to the basket
            {
                var cart = new ShoppingBasket { UserId = user.Id, ProductsId = productId, Amount = Amount, DateCreated = DateTime.Now};
                _context.Basket.Add(cart);
                _context.SaveChanges();
            }


            return Json(new { success = true });
        }

        //decreases or removes items from shopping cart
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id, float price)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var cartItem = _context.Basket.FirstOrDefault(x => x.ProductsId == id && x.UserId == user.Id);
                int itemCount = 0;

                if (cartItem != null)
                {
                    if (cartItem.Amount > 1)
                    {
                        cartItem.Amount--;
                        itemCount = cartItem.Amount;
                    }
                    else
                    {
                        _context.Basket.Remove(cartItem);
                    }

                    _context.SaveChanges(); // Save changes
                }

                var results = new ShoppingCartRemoveViewModel
                {

                    DeleteId = id,
                    ItemCount = itemCount,
                    CartTotal = Math.Round((itemCount * price), 2, MidpointRounding.AwayFromZero)

                };
                return Json(results);
            }

            return Json(new {success = true});
        }

        [HttpGet]
        public IActionResult CheckOut()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(List<ProductsShopCartViewModel> vm)
        {
            List<ProductsShopCartViewModel> ShoppingCart = new List<ProductsShopCartViewModel>();

            foreach (var product in vm)
            {
                var dbProduct = _context.products.Where(x => x.ProductId == product.ProductId).Select(x => x).SingleOrDefault();
                
                ShoppingCart.Add(new ProductsShopCartViewModel{
                        ProductId = dbProduct.ProductId,
                        Amount = product.Amount,
                        Name = dbProduct.Name,
                        ImageUrl = dbProduct.ImageUrl,
                        Price = dbProduct.Price,
                        Grade = dbProduct.Grade,
                        TotalPrice = (dbProduct.Price * product.Amount)

                    }
                
                );

            }

            _context.SaveChanges();
            return Json(ShoppingCart);
        }




        [HttpPost]
        public async Task<ActionResult> AddLocalCartToDatabase(List<ProductsShopCartViewModel> vm)
        {
            var user = await _userManager.GetUserAsync(User);
            var cartproducts = _assets.ShoppingbasketByName(user.Id).ToList();
            foreach (var product in vm)
            {
                if (!cartproducts.Select(x => x.ProductsId).Contains(product.ProductId))
                {
                    var cart = new ShoppingBasket
                    {

                        Amount = product.Amount,
                        DateCreated = DateTime.Now,
                        UserId = user.Id,
                        ProductsId = product.ProductId

                    };
                    _context.Basket.Add(cart);
                }
                else
                {
                    if (cartproducts.Select(x => x.ProductsId).Contains(product.ProductId))
                    {
                        ShoppingBasket updatedmodel = cartproducts.FirstOrDefault(x => x.ProductsId == product.ProductId);
                        updatedmodel.Amount = product.Amount;
                        _context.Update(updatedmodel);
                    }

                }
            }
            _context.SaveChanges();
            return Json(new { success = true });
        }


        public async Task<ActionResult> AddDbCarttoLocal()
        {
            var user = await _userManager.GetUserAsync(User);
            var cartproducts = _assets.ShoppinCartItems(user.Id).ToList();

            return Json(cartproducts);
        }


        public async Task<IActionResult> SetAmountinShoppingCart(int id, int amount)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var cartItem = _context.Basket.FirstOrDefault(x => x.ProductsId == id && x.UserId == user.Id);

                if (cartItem != null)
                {

                    if (amount < 1)
                    {
                        _context.Basket.Remove(cartItem);
                    }
                    else
                    {
                        cartItem.Amount = amount;
                    }

                    _context.SaveChanges(); // Save changes
                }
            }

            return Json(new { success = true });
        }
    }
}
