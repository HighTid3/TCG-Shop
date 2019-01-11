using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.Services;
using TCGshopTestEnvironment.ViewModels;

namespace TCGshopTestEnvironment.Controllers
{
    public class AuctionController : Controller
    {
        private DBModel _context;
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
            var AuctionBid = new AuctionBids
                {Bid = Bid, BidDate = DateTime.Now, ProductId = productId, UserId = user.Id};
            _context.AuctionBids.Add(AuctionBid);
            _context.SaveChanges();
            return Json(new { success = true });
        }
    }
}
