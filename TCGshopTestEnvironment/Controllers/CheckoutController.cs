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
    public class CheckoutController : Controller
    {
        private DBModel _context;
        private IShopping _assets;
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;

        [HttpGet]
        public IActionResult Start()
        {
            //TODO
            //Logic for logged in users

            //For guest
            return View();
        }

        [HttpGet]
        public ActionResult AccountAndAddress()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> AccountAndAddress([FromBody] AccountAndAddressViewModel OrderDetails)
        {
            return Ok(OrderDetails);

            //var values = OrderView;
            //var order = new Order();
            //var user = await _userManager.GetUserAsync(User);

            //try
            //{
            //    order.Email = user.Email;
            //    order.OrderDate = DateTime.Now;
            //    order.FirstName = values.FirstName;
            //    order.LastName = values.FirstName;
            //    order.Address = values.Address;
            //    order.City = values.City;
            //    order.State = values.State;
            //    order.PostalCode = values.PostalCode;
            //    order.Country = values.Country;




            //    _context.Orders.Add(order);
            //    return Ok();

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    return Ok();
            //}

        }








        // POST: /Checkout/AddressAndPayment
        //[HttpPost]
        //public ActionResult AddressAndPayment(FormCollection values)
        //{
        //    var order = new Order();

        //    try
        //    {
        //        if (string.Equals(values["PromoCode"], PromoCode,
        //                StringComparison.OrdinalIgnoreCase) == false)
        //        {
        //            return View(order);
        //        }
        //        else
        //        {
        //            order.Username = User.Identity.Name;
        //            order.OrderDate = DateTime.Now;

        //            //Save Order
        //            storeDB.Orders.Add(order);
        //            storeDB.SaveChanges();
        //            //Process the order
        //            var cart = ShoppingCart.GetCart(this.HttpContext);
        //            cart.CreateOrder(order);

        //            return RedirectToAction("Complete",
        //                new { id = order.OrderId });
        //        }
        //    }
        //    catch
        //    {
        //        //Invalid - redisplay with errors
        //        return View(order);
        //    }
        //}


    }
}
