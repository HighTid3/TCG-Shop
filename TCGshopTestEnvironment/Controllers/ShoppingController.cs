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


        //public ActionResult Add(ProductsShopCartViewModel product)
        //{
        //    if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
        //    {
        //        List<ProductsShopCartViewModel> CartProducts = new List<ProductsShopCartViewModel>();

        //        CartProducts.Add(product);
        //        HttpContext.Session.SetObjectAsJson(SessionKeyName,CartProducts);
        //        ViewBag.cart = CartProducts.Count();

        //        HttpContext.Session.SetInt32(TotalCartProducts, 1);

        //    }

        //    else
        //    {
        //        List<ProductsShopCartViewModel> CartProducts = HttpContext.Session.GetObjectFromJson<List<ProductsShopCartViewModel>>(SessionKeyName);
        //        CartProducts.Add(product);
        //        HttpContext.Session.SetObjectAsJson(SessionKeyName, CartProducts);
        //        ViewBag.cart = CartProducts.Count();

        //        var count = HttpContext.Session.GetInt32(TotalCartProducts);

        //        HttpContext.Session.SetInt32(TotalCartProducts, Convert.ToInt32(HttpContext.Session.GetInt32(TotalCartProducts) + 1) );

        //    }

        //    return RedirectToAction("Index", "Home");
        //}

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
                var cartproducts = _assets.ShoppinCartItems(user.UserName).ToList();
                return View(cartproducts);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddToShoppingcart(string userId, int productId, int Amount)
        {
            var user = await _userManager.GetUserAsync(User);
            if (userId != user.UserName)
            {
                return Json(new { error = "userId did not match with the session" });
            }
            var assetModel = _assets.ShoppingbasketByName(userId).ToList();
            if (assetModel.Select(x => x.ProductsId).Contains(productId))
            {

                ShoppingBasket updatedmodel = assetModel.FirstOrDefault(x => x.ProductsId == productId);
                updatedmodel.Amount += Amount;
                _context.Update(updatedmodel);
                _context.SaveChanges();

            }
            else
            {
                var cart = new ShoppingBasket { UserId = userId, ProductsId = productId, Amount = Amount, DateCreated = DateTime.Now};
                _context.Basket.Add(cart);
                _context.SaveChanges();
            }


            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id, float price)
        {
            var cartItem = _context.Basket.FirstOrDefault(x => x.Id == id);
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
                // Save changes
                _context.SaveChanges();
            }
            var results = new ShoppingCartRemoveViewModel
            {

                DeleteId = id,
                ItemCount = itemCount,
                CartTotal = Math.Round((itemCount * price),2, MidpointRounding.AwayFromZero)
               
                
            };
            return Json(results);
        }

        [HttpGet]
        public IActionResult CheckOut()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(ProductsShopCartViewModel vm)
        {

            return View();
        }
    }

    //public static class SessionExtensions
    //{
    //    public static void SetObjectAsJson(this ISession session, string key, object value)
    //    {
    //        session.SetString(key, JsonConvert.SerializeObject(value));
    //    }

    //    public static T GetObjectFromJson<T>(this ISession session, string key)
    //    {
    //        var value = session.GetString(key);

    //        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    //    }
    //}



}
