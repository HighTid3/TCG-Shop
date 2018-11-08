using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.ViewModels;

namespace TCGshopTestEnvironment.Controllers
{
    public class ShoppingController : Controller
    {
        public const string SessionKeyName = "_Cart";
        public const string TotalCartProducts = "_count";
        private DBModel _context;

        
        public ShoppingController(DBModel context)
        {
            _context = context;
        }

        public ActionResult Add(ProductsShopCartViewModel product)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                List<ProductsShopCartViewModel> CartProducts = new List<ProductsShopCartViewModel>();

                CartProducts.Add(product);
                HttpContext.Session.SetObjectAsJson(SessionKeyName,CartProducts);
                ViewBag.cart = CartProducts.Count();

                HttpContext.Session.SetInt32(TotalCartProducts, 1);

            }

            else
            {
                List<ProductsShopCartViewModel> CartProducts = HttpContext.Session.GetObjectFromJson<List<ProductsShopCartViewModel>>(SessionKeyName);
                CartProducts.Add(product);
                HttpContext.Session.SetObjectAsJson(SessionKeyName, CartProducts);
                ViewBag.cart = CartProducts.Count();

                var count = HttpContext.Session.GetInt32(TotalCartProducts);

                HttpContext.Session.SetInt32(TotalCartProducts, Convert.ToInt32(HttpContext.Session.GetInt32(TotalCartProducts) + 1) );

            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ShoppingCart()
        {

            return View();
        }



    }

    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }



}
