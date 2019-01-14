using System.Text.Encodings.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.Services;
using TCGshopTestEnvironment.ViewModels;
using Microsoft.AspNetCore.Mvc.Routing;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;

namespace TCGshopTestEnvironment.Controllers
{
    public class AuctionController : Controller
    {
        private DBModel _context;
        private IPaymentClient paymentClient = new PaymentClient("test_y7mvmmrTRRRJECQxvSwrBSJHdjKuxa");
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IProducts _assets;
        private readonly IAuction _auction;
        public AuctionController(DBModel context, UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager, IEmailSender emailSender, IProducts assets, IAuction auction)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _assets = assets;
            _auction = auction;
        }

        [HttpGet]
        public IActionResult AuctionHouse()
        {
            var model = _auction.GetAuctionCards();
            return View(model);
        }

        [HttpGet]
        public IActionResult AuctionDetails(int id)
        {
            var model = _auction.GetByID(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AuctionDetails(decimal Bid, int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            var HighestBid = _context.AuctionBids.Where(x => x.ProductId == productId).Select(x => x.Bid).DefaultIfEmpty().Max();
            if (HighestBid >= Bid)
            {
                return Json("test");
            }
            else
            {
                var secondhighestbidder = _context.AuctionBids.Where(x => x.ProductId == productId)
                    .OrderByDescending(r => r.Bid).Select(x => x.User).Skip(1).FirstOrDefault();
                if (secondhighestbidder != null)
                {
                    await _emailSender.SendEmailAsync(secondhighestbidder.Email, "You have been Outbidden",
                        "You have been outbidden on a action you made a bid on");
                }
                var AuctionBid = new AuctionBids
                { Bid = Convert.ToDecimal(Bid.ToString("F")), BidDate = DateTime.Now, ProductId = productId, UserId = user.Id };
                _context.AuctionBids.Add(AuctionBid);
                _context.SaveChanges();
                return Json(new { success = true });
            }

        }

        [HttpPost]
        public async Task<IActionResult> RewardEmail()
        {
            //check which auctions have ended
            var EndedAuctions = _context.products.Where(x => x.AuctionEndTime < DateTime.Now && x.AuctionEndTime > new DateTime(2017) && !x.Removed).ToList();


            var highestBid = new List<HighestBidDetails> { };

            //fill highestbid list with users that have the highest bids on their auctions
            foreach (var Highestbiddings in EndedAuctions)
            {
                var userbid = _context.AuctionBids.Where(x => x.ProductId == Highestbiddings.ProductId)
                    .DefaultIfEmpty().FirstOrDefault();
                var userofbid = _context.AuctionBids.Where(x => x.ProductId == Highestbiddings.ProductId).Select(x => x.User)
                    .DefaultIfEmpty().FirstOrDefault();
                var highbidID = new HighestBidDetails
                {
                    Bid = userbid.Bid,
                    product = userbid.Product,
                    User = userofbid
                };
                if (userbid != null) highestBid.Add(highbidID);
                Highestbiddings.Removed = true;
            }

            _context.SaveChanges();



            foreach (var user in highestBid)
            {
                var order = new Order
                {
                    PaymentStatus = "Waiting for payment",
                    Address = user.User.Address,
                    Country = user.User.Country,
                    Email = user.User.Email,
                    FirstName = user.User.FirstName,
                    Guid = Guid.NewGuid(),
                    LastName = user.User.LastName,
                    OrderDate = DateTime.Now,
                    Paid = false,
                    Phone = user.User.PhoneNumber,
                    PostalCode = user.User.ZipCode,
                    Total = user.Bid

                };
                List<OrderDetail> OrderDetail = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = user.product.ProductId,
                        Quantity = 1,
                        UnitPrice = user.Bid,
                        Product = user.product,
                        Order = order
                    }
                };
                order.OrderDetails = OrderDetail;

                PaymentRequest paymentRequest = new PaymentRequest()
                {
                    Amount = new Amount(Currency.EUR, order.Total.ToString("F", CultureInfo.InvariantCulture)),
                    Description = "Payment for your mock purchase from TCG.Sale",
                    RedirectUrl = "https://tcg.sale" + Url.Action("Processing", "Checkout") + "?guid=" + order.Guid,
                    WebhookUrl = "https://tcg.sale" + Url.Action("Webhook", "Checkout")
                };

                PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);
                Console.WriteLine(paymentResponse.Links.Checkout);

                //Updating PaymentId
                order.PaymentId = paymentResponse.Id;
                order.PaymentStatus = "created";

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                //var orderurl = "tcg.sale/Manage/OrderDetails?orderid=" + order.OrderId +"";
                await _emailSender.SendEmailAsync(user.User.Email, "Auction Won!",
                    "You have won the auction, you can pay by clicking on this link: "+
                    paymentResponse.Links.Checkout.Href+ " for further details about the card you won, Go to your order page on our website")
                    ;
            }




            return RedirectToAction("AuctionHouse", "Auction");


            //return View(vm);
        }
    }

    public class HighestBidDetails
    {
        public Products product { get; set; }
        public UserAccount User { get; set; }
        public decimal Bid { get; set; }
    }

}
