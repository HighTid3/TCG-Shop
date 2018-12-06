﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.Services;
using TCGshopTestEnvironment.ViewModels;

namespace TCGshopTestEnvironment.Controllers
{
    public class CheckoutController : Controller
    {
        private IPaymentClient paymentClient = new PaymentClient("test_y7mvmmrTRRRJECQxvSwrBSJHdjKuxa");
        private DBModel _context;
        private IShopping _assets;
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;

        public CheckoutController(DBModel context, IShopping assets, UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager)
        {
            _context = context;
            _assets = assets;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Start()
        {
            
            //TODO
            //Logic for logged in users

            //For guest
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> AccountAndAddress()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var model = new CheckoutViewModel
                {
                    Address = user.Address,
                    Country = user.Country,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PostalCode = user.ZipCode,


                };
                return PartialView(model);
            }
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> AccountAndAddress([FromBody] AccountAndAddressViewModel OrderDetails)
        {
            //return Ok(OrderDetails);

            var values = OrderDetails.OrderViewModel;
            var order = new Order();
            //var user = await _userManager.GetUserAsync(User);

            try
            {
                //Creating Order
                //order.Email = user.Email; //Get From User Account
                order.Email = values[0].Email; //Get From Form
                order.Guid = Guid.NewGuid();
                order.OrderDate = DateTime.Now;
                order.FirstName = values[0].FirstName;
                order.LastName = values[0].FirstName;
                order.Address = values[0].Street + " " + values[0].HouseNumber;
                order.City = values[0].City;
                order.State = values[0].State;
                order.PostalCode = values[0].PostalCode;
                order.Country = values[0].Country;
                order.Paid = false;
                //order.OrderDetails = OrderDetails.OrderDetails;

                decimal Total = 0;

                List<OrderDetail> OrderDetail = new List<OrderDetail>(); //Creating Empty List of Orders

                //Adding Products to Order
                foreach (var product in OrderDetails.OrderDetails)
                {
                    //Get Product from DB, to make sure the price is up to date
                    var dbProduct = _context.products.Where(x => x.ProductId == product.ProductId).Select(x => x).SingleOrDefault();

                    OrderDetail.Add(new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = dbProduct.ProductId,
                        Quantity = product.Amount,
                        UnitPrice = dbProduct.Price,
                        Product = dbProduct,
                        Order = order
                    }

                    );

                    //Updating Total
                    Total = Total + product.Amount * dbProduct.Price;
                }

                order.Total = Total;
                order.OrderDetails = OrderDetail;

                //Now We Need To Create A Mollie Payment Token
                PaymentRequest paymentRequest = new PaymentRequest()
                {
                    Amount = new Amount(Currency.EUR, Total.ToString("F", CultureInfo.InvariantCulture)),
                    Description = "Payment for your mock purchase from TCG.Sale",
                    RedirectUrl = "https://tcg.sale" + Url.Action("Processing", "Checkout") + "?guid=" + order.Guid,
                    WebhookUrl = "https://tcg.sale" + Url.Action("Webhook", "Checkout")
                };

                PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);
                Console.WriteLine(paymentResponse.Links.Checkout);

                //Updating PaymentId
                order.PaymentId = paymentResponse.Id;
                order.PaymentStatus = "created";

                //Writing Order to DB
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return Json(paymentResponse.Links.Checkout.Href);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Ok();
            }
        }

        [HttpGet]
        public ActionResult Processing(Guid guid)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ProcessingStatus(Guid guid)
        {
            //Search for the Order Posted by the Webhook
            var dbOrder = _context.Orders.Where(x => x.Guid == guid).Select(x => x).SingleOrDefault();
            try
            {
                return Json(new { status = dbOrder.PaymentStatus });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Webhook(string id)
        {
            //Search for the Order Posted by the Webhook
            var dbOrder = _context.Orders.Where(x => x.PaymentId == id).Select(x => x).SingleOrDefault();

            //Get the current Payment Status
            PaymentResponse result = await paymentClient.GetPaymentAsync(id);

            //Update dbOrder
            dbOrder.PaymentStatus = result.Status.ToString();
            _context.Update(dbOrder);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}