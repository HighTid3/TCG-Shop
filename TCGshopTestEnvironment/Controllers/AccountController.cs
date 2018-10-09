using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.ViewModels;
using TCGshopTestEnvironment.Controllers;
using TCGshopTestEnvironment.Services;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TCGshopTestEnvironment.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }


        [HttpGet]
        public IActionResult Login()
        {
            //_emailSender.SendEmailAsync("jurre@koetse.eu", "Please Work","Please work by clicking here.");

            ViewBag.Title = "Login Page";
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, vm.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Username or password is incorrect.");
                return View(vm);
            }

            return View(vm);
        }



        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {

                var user = new UserAccount
                {
                    UserName = vm.UserName,
                    Email = vm.Email,
                    ZipCode = vm.ZipCode,
                    Country = vm.Country,
                    Address = vm.Address,
                    PhoneNumber = vm.PhoneNumber,
                    LastName = vm.LastName,
                    FirstName = vm.FirstName
                        
                    
                };
                var result = await _userManager.CreateAsync(user, vm.Password);

                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);


                    await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                        "Please confirm your account by <a href=" + callbackUrl + ">clicking here</a>.");

                    // await _signInManager.SignInAsync(user, isPersistent: false);
                    //return LocalRedirect(returnUrl);



                    //await _signInManager.SignInAsync(user, false);
                    //return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    
                }
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }

}
