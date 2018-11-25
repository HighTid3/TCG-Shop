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
using Microsoft.AspNetCore.Mvc.Routing;
using System.ComponentModel.DataAnnotations;
using PostcodeAPI;
using PostcodeAPI.Wrappers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TCGshopTestEnvironment.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly IEmailSender _emailSender;

        //Dit is voor de Postcode naar straat api. (We hebben een limiet van 100 calls)
        // Instantiate the client with your API key
        private readonly PostcodeApiClient _postcodeApiClient = new PostcodeApiClient("K95SESr2l162I7u1KxQn61zEc1s36rak5j3SAld9"); 


        public AccountController(UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");
            }

            return View();
        }

        // /Account/ForgetPassword
        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPasswordConfirmation()
        {

            return View();
        }

        // Model for Forget Password

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(vm.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var callbackUrl = Url.Page(
                //    "/Account/ResetPassword",
                //    pageHandler: null,
                //    values: new { code },
                //    protocol: Request.Scheme);

                var callbackUrl = Url.Action(new UrlActionContext
                {
                    Action = "ResetPassword",
                    Controller = "Account",
                    Values = new { code = code },
                    Protocol = HttpContext.Request.Scheme
                });


                await _emailSender.SendEmailAsync(
                    vm.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                //return RedirectToPage("./ForgotPasswordConfirmation");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            return View();
        }

        public ResetPasswordViewModel RpInput { get; set; }


        //Reset Password
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                RpInput = new ResetPasswordViewModel
                {
                    Code = code
                };
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            var result = await _userManager.ResetPasswordAsync(user, vm.Code, vm.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ResetPasswordConfirmation()
        {

            return View();
        }




        [HttpGet]
        public IActionResult Login()
        {
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
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { userId = user.Id, code = code },
                    //    protocol: Request.Scheme);

                    var callbackUrl = Url.Action(new UrlActionContext
                    {
                        Action = "ConfirmEmail",
                        Controller = "Account",
                        Values = new { userId = user.Id, code = code },
                        Protocol = HttpContext.Request.Scheme
                    });



                    await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                        "Please confirm your account by <a href=" + callbackUrl + ">clicking here</a>.");


                    return RedirectToAction("Index", "Home");
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

        //PostCode -> Straat API.
        [HttpGet]
        public IActionResult PostCodeApi(string postcode, int huisnummer)
        {
            ApiHalResultWrapper result = _postcodeApiClient.GetAddress(postcode, huisnummer);
            return Json(result);

        }

        [HttpGet]
        public IActionResult PostCodeApiFake(string postcode, int huisnummer)
        {
            string fake =
                "{\"_embedded\":{\"addresses\":[{\"id\":\"0568200000301455\",\"street\":\"Boezempad\",\"number\":5,\"letter\":null,\"addition\":null,\"postcode\":\"3216AD\",\"surface\":110,\"nen5825\":{\"street\":\"BOEZEMPAD\",\"postcode\":\"3216 AD\"},\"city\":{\"id\":\"1385\",\"label\":\"Abbenbroek\"},\"municipality\":{\"id\":\"1930\",\"label\":\"Nissewaard\"},\"province\":{\"id\":\"28\",\"label\":\"Zuid-Holland\"},\"geo\":{\"center\":{\"wgs84\":{\"coordinates\":[4.2413365,51.8504566],\"type\":\"Point\",\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:OGC:1.3:CRS84\"}}},\"rd\":{\"type\":\"Point\",\"coordinates\":[76051.0,429720.0],\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:EPSG::28992\"}}}},\"exterior\":null},\"type\":\"Verblijfsobject\",\"purpose\":\"woonfunctie\",\"year\":1994,\"_links\":{\"self\":{\"href\":\"https://postcode-api.apiwise.nl/v2/addresses/0568200000301455/\"},\"next\":null}}],\"postcodes\":null},\"_links\":{\"self\":{\"href\":\"https://postcode-api.apiwise.nl/v2/addresses/?postcode=3216AD&number=5\"},\"next\":null}}";
            return Content(fake);
        }

        [HttpGet]
        public IActionResult PostCodeApiFakeLong(string postcode, int huisnummer)
        {
            string fake =
                "{\"_embedded\":{\"addresses\":[{\"id\":\"0568200000303240\",\"street\":\"Boezempad\",\"number\":1,\"letter\":null,\"addition\":null,\"postcode\":\"3216AD\",\"surface\":135,\"nen5825\":{\"street\":\"BOEZEMPAD\",\"postcode\":\"3216 AD\"},\"city\":{\"id\":\"1385\",\"label\":\"Abbenbroek\"},\"municipality\":{\"id\":\"1930\",\"label\":\"Nissewaard\"},\"province\":{\"id\":\"28\",\"label\":\"Zuid-Holland\"},\"geo\":{\"center\":{\"wgs84\":{\"coordinates\":[4.2416472,51.850226],\"type\":\"Point\",\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:OGC:1.3:CRS84\"}}},\"rd\":{\"type\":\"Point\",\"coordinates\":[76072.0,429694.0],\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:EPSG::28992\"}}}},\"exterior\":null},\"type\":\"Verblijfsobject\",\"purpose\":\"woonfunctie\",\"year\":1994,\"_links\":{\"self\":{\"href\":\"https://postcode-api.apiwise.nl/v2/addresses/0568200000303240/\"},\"next\":null}},{\"id\":\"0568200000305276\",\"street\":\"Boezempad\",\"number\":2,\"letter\":null,\"addition\":null,\"postcode\":\"3216AD\",\"surface\":101,\"nen5825\":{\"street\":\"BOEZEMPAD\",\"postcode\":\"3216 AD\"},\"city\":{\"id\":\"1385\",\"label\":\"Abbenbroek\"},\"municipality\":{\"id\":\"1930\",\"label\":\"Nissewaard\"},\"province\":{\"id\":\"28\",\"label\":\"Zuid-Holland\"},\"geo\":{\"center\":{\"wgs84\":{\"coordinates\":[4.2413185,51.850025],\"type\":\"Point\",\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:OGC:1.3:CRS84\"}}},\"rd\":{\"type\":\"Point\",\"coordinates\":[76049.0,429672.0],\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:EPSG::28992\"}}}},\"exterior\":null},\"type\":\"Verblijfsobject\",\"purpose\":\"woonfunctie\",\"year\":1966,\"_links\":{\"self\":{\"href\":\"https://postcode-api.apiwise.nl/v2/addresses/0568200000305276/\"},\"next\":null}},{\"id\":\"0568200000301634\",\"street\":\"Boezempad\",\"number\":3,\"letter\":null,\"addition\":null,\"postcode\":\"3216AD\",\"surface\":114,\"nen5825\":{\"street\":\"BOEZEMPAD\",\"postcode\":\"3216 AD\"},\"city\":{\"id\":\"1385\",\"label\":\"Abbenbroek\"},\"municipality\":{\"id\":\"1930\",\"label\":\"Nissewaard\"},\"province\":{\"id\":\"28\",\"label\":\"Zuid-Holland\"},\"geo\":{\"center\":{\"wgs84\":{\"coordinates\":[4.2415134,51.8503505],\"type\":\"Point\",\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:OGC:1.3:CRS84\"}}},\"rd\":{\"type\":\"Point\",\"coordinates\":[76063.0,429708.0],\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:EPSG::28992\"}}}},\"exterior\":null},\"type\":\"Verblijfsobject\",\"purpose\":\"woonfunctie\",\"year\":1994,\"_links\":{\"self\":{\"href\":\"https://postcode-api.apiwise.nl/v2/addresses/0568200000301634/\"},\"next\":null}},{\"id\":\"0568200000304036\",\"street\":\"Boezempad\",\"number\":4,\"letter\":null,\"addition\":null,\"postcode\":\"3216AD\",\"surface\":101,\"nen5825\":{\"street\":\"BOEZEMPAD\",\"postcode\":\"3216 AD\"},\"city\":{\"id\":\"1385\",\"label\":\"Abbenbroek\"},\"municipality\":{\"id\":\"1930\",\"label\":\"Nissewaard\"},\"province\":{\"id\":\"28\",\"label\":\"Zuid-Holland\"},\"geo\":{\"center\":{\"wgs84\":{\"coordinates\":[4.2412453,51.8500512],\"type\":\"Point\",\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:OGC:1.3:CRS84\"}}},\"rd\":{\"type\":\"Point\",\"coordinates\":[76044.0,429675.0],\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:EPSG::28992\"}}}},\"exterior\":null},\"type\":\"Verblijfsobject\",\"purpose\":\"woonfunctie\",\"year\":1966,\"_links\":{\"self\":{\"href\":\"https://postcode-api.apiwise.nl/v2/addresses/0568200000304036/\"},\"next\":null}},{\"id\":\"0568200000301455\",\"street\":\"Boezempad\",\"number\":5,\"letter\":null,\"addition\":null,\"postcode\":\"3216AD\",\"surface\":110,\"nen5825\":{\"street\":\"BOEZEMPAD\",\"postcode\":\"3216 AD\"},\"city\":{\"id\":\"1385\",\"label\":\"Abbenbroek\"},\"municipality\":{\"id\":\"1930\",\"label\":\"Nissewaard\"},\"province\":{\"id\":\"28\",\"label\":\"Zuid-Holland\"},\"geo\":{\"center\":{\"wgs84\":{\"coordinates\":[4.2413365,51.8504566],\"type\":\"Point\",\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:OGC:1.3:CRS84\"}}},\"rd\":{\"type\":\"Point\",\"coordinates\":[76051.0,429720.0],\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:EPSG::28992\"}}}},\"exterior\":null},\"type\":\"Verblijfsobject\",\"purpose\":\"woonfunctie\",\"year\":1994,\"_links\":{\"self\":{\"href\":\"https://postcode-api.apiwise.nl/v2/addresses/0568200000301455/\"},\"next\":null}},{\"id\":\"0568200000305732\",\"street\":\"Boezempad\",\"number\":6,\"letter\":null,\"addition\":null,\"postcode\":\"3216AD\",\"surface\":101,\"nen5825\":{\"street\":\"BOEZEMPAD\",\"postcode\":\"3216 AD\"},\"city\":{\"id\":\"1385\",\"label\":\"Abbenbroek\"},\"municipality\":{\"id\":\"1930\",\"label\":\"Nissewaard\"},\"province\":{\"id\":\"28\",\"label\":\"Zuid-Holland\"},\"geo\":{\"center\":{\"wgs84\":{\"coordinates\":[4.2411287,51.8500681],\"type\":\"Point\",\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:OGC:1.3:CRS84\"}}},\"rd\":{\"type\":\"Point\",\"coordinates\":[76036.0,429677.0],\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:EPSG::28992\"}}}},\"exterior\":null},\"type\":\"Verblijfsobject\",\"purpose\":\"woonfunctie\",\"year\":1966,\"_links\":{\"self\":{\"href\":\"https://postcode-api.apiwise.nl/v2/addresses/0568200000305732/\"},\"next\":null}},{\"id\":\"0568200000300696\",\"street\":\"Boezempad\",\"number\":7,\"letter\":null,\"addition\":null,\"postcode\":\"3216AD\",\"surface\":131,\"nen5825\":{\"street\":\"BOEZEMPAD\",\"postcode\":\"3216 AD\"},\"city\":{\"id\":\"1385\",\"label\":\"Abbenbroek\"},\"municipality\":{\"id\":\"1930\",\"label\":\"Nissewaard\"},\"province\":{\"id\":\"28\",\"label\":\"Zuid-Holland\"},\"geo\":{\"center\":{\"wgs84\":{\"coordinates\":[4.2412418,51.8504737],\"type\":\"Point\",\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:OGC:1.3:CRS84\"}}},\"rd\":{\"type\":\"Point\",\"coordinates\":[76044.5,429722.0],\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:EPSG::28992\"}}}},\"exterior\":null},\"type\":\"Verblijfsobject\",\"purpose\":\"woonfunctie\",\"year\":1994,\"_links\":{\"self\":{\"href\":\"https://postcode-api.apiwise.nl/v2/addresses/0568200000300696/\"},\"next\":null}},{\"id\":\"0568200000304540\",\"street\":\"Boezempad\",\"number\":8,\"letter\":null,\"addition\":null,\"postcode\":\"3216AD\",\"surface\":101,\"nen5825\":{\"street\":\"BOEZEMPAD\",\"postcode\":\"3216 AD\"},\"city\":{\"id\":\"1385\",\"label\":\"Abbenbroek\"},\"municipality\":{\"id\":\"1930\",\"label\":\"Nissewaard\"},\"province\":{\"id\":\"28\",\"label\":\"Zuid-Holland\"},\"geo\":{\"center\":{\"wgs84\":{\"coordinates\":[4.2410702,51.8500855],\"type\":\"Point\",\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:OGC:1.3:CRS84\"}}},\"rd\":{\"type\":\"Point\",\"coordinates\":[76032.0,429679.0],\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:EPSG::28992\"}}}},\"exterior\":null},\"type\":\"Verblijfsobject\",\"purpose\":\"woonfunctie\",\"year\":1966,\"_links\":{\"self\":{\"href\":\"https://postcode-api.apiwise.nl/v2/addresses/0568200000304540/\"},\"next\":null}},{\"id\":\"0568200000304660\",\"street\":\"Boezempad\",\"number\":9,\"letter\":null,\"addition\":null,\"postcode\":\"3216AD\",\"surface\":144,\"nen5825\":{\"street\":\"BOEZEMPAD\",\"postcode\":\"3216 AD\"},\"city\":{\"id\":\"1385\",\"label\":\"Abbenbroek\"},\"municipality\":{\"id\":\"1930\",\"label\":\"Nissewaard\"},\"province\":{\"id\":\"28\",\"label\":\"Zuid-Holland\"},\"geo\":{\"center\":{\"wgs84\":{\"coordinates\":[4.241035,51.8503278],\"type\":\"Point\",\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:OGC:1.3:CRS84\"}}},\"rd\":{\"type\":\"Point\",\"coordinates\":[76030.0,429706.0],\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:EPSG::28992\"}}}},\"exterior\":null},\"type\":\"Verblijfsobject\",\"purpose\":\"woonfunctie\",\"year\":1994,\"_links\":{\"self\":{\"href\":\"https://postcode-api.apiwise.nl/v2/addresses/0568200000304660/\"},\"next\":null}},{\"id\":\"0568200000304171\",\"street\":\"Boezempad\",\"number\":10,\"letter\":null,\"addition\":null,\"postcode\":\"3216AD\",\"surface\":101,\"nen5825\":{\"street\":\"BOEZEMPAD\",\"postcode\":\"3216 AD\"},\"city\":{\"id\":\"1385\",\"label\":\"Abbenbroek\"},\"municipality\":{\"id\":\"1930\",\"label\":\"Nissewaard\"},\"province\":{\"id\":\"28\",\"label\":\"Zuid-Holland\"},\"geo\":{\"center\":{\"wgs84\":{\"coordinates\":[4.2410108,51.8501388],\"type\":\"Point\",\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:OGC:1.3:CRS84\"}}},\"rd\":{\"type\":\"Point\",\"coordinates\":[76028.0,429685.0],\"crs\":{\"type\":\"name\",\"properties\":{\"name\":\"urn:ogc:def:crs:EPSG::28992\"}}}},\"exterior\":null},\"type\":\"Verblijfsobject\",\"purpose\":\"woonfunctie\",\"year\":1966,\"_links\":{\"self\":{\"href\":\"https://postcode-api.apiwise.nl/v2/addresses/0568200000304171/\"},\"next\":null}}],\"postcodes\":null},\"_links\":{\"self\":{\"href\":\"https://postcode-api.apiwise.nl/v2/addresses/?postcode=3216AD&number=0\"},\"next\":null}}";
            return Content(fake);
        }
    }

}

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860