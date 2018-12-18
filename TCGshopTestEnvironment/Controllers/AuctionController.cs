using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.Services;

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

    }
}
