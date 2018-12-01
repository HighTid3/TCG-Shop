using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.Services;
using TCGshopTestEnvironment.ViewModels.ManageViewModels;

namespace TCGshopTestEnvironment.Controllers
{
    public class ManageController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IManage _manage;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly UserManager<UserAccount> _userManager;

        public ManageController(
            UserManager<UserAccount> userManager,
            ILogger<ManageController> logger,
            SignInManager<UserAccount> signInManager,
            IEmailSender emailSender,
            IManage manage)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _manage = manage;
        }

        [TempData] public string StatusMessage { get; set; }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = StatusMessage,
                Address = user.Address,
                Country = user.Country,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ZipCode = user.ZipCode
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var email = user.Email;
            if (model.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                    throw new ApplicationException(
                        $"Unexpected error occurred setting email for user with ID '{user.Id}'.");
            }

            var phoneNumber = user.PhoneNumber;
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                    throw new ApplicationException(
                        $"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
            }

            var firstName = user.FirstName;
            if (model.FirstName != firstName)
            {
                user.FirstName = model.FirstName;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new ApplicationException(
                        $"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
            }

            var lastName = user.LastName;
            if (model.LastName != lastName)
            {
                user.LastName = model.LastName;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new ApplicationException(
                        $"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
            }

            var country = user.Country;
            if (model.Country != country)
            {
                user.Country = model.Country;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new ApplicationException(
                        $"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
            }

            var address = user.Address;
            if (model.Address != address)
            {
                user.Address = model.Address;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new ApplicationException(
                        $"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
            }

            var zipcode = user.ZipCode;
            if (model.ZipCode != zipcode)
            {
                user.ZipCode = model.ZipCode;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new ApplicationException(
                        $"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
            }

            StatusMessage = "Your profile has been updated";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var model = new ChangePasswordViewModel {StatusMessage = StatusMessage};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var changePasswordResult =
                await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await _signInManager.SignInAsync(user, false);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Your password has been changed.";

            return RedirectToAction(nameof(ChangePassword));
        }

        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");


            var model = _manage.OrderOverview(user.Email);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetails(int orderid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");


            var model = _manage.Orderdetails(user.Email, orderid);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UserManagement()
        {
            var asset = _manage.GetRegisteredUsers();

            var model = asset.Select(result => new UserManagementViewModel
            {
                Email = result.Email,
                EmailConfirmed = result.EmailConfirmed,
                Username = result.UserName
            });
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UserDetails(string username)
        {
            var asset = _manage.GetRegisteredUserbyUsername(username);

            var model = new UserManagementDetailsViewModel
            {
                Email = asset.Email,
                EmailConfirmed = asset.EmailConfirmed,
                Username = asset.UserName,
                Address = asset.Address,
                Country = asset.Country,
                FirstName = asset.FirstName,
                LastName = asset.LastName,
                PhoneNumber = asset.PhoneNumber,
                ZipCode = asset.ZipCode
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UserEdit(string username)
        {
            var asset = _manage.GetRegisteredUserbyUsername(username);

            var model = new UserManagementDetailsViewModel
            {
                Email = asset.Email,
                EmailConfirmed = asset.EmailConfirmed,
                Username = asset.UserName,
                Address = asset.Address,
                Country = asset.Country,
                FirstName = asset.FirstName,
                LastName = asset.LastName,
                PhoneNumber = asset.PhoneNumber,
                ZipCode = asset.ZipCode
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEdit(UserManagementDetailsViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _userManager.FindByNameAsync(vm.Username);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var email = user.Email;
            if (vm.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, vm.Email);
                if (!setEmailResult.Succeeded)
                    throw new ApplicationException(
                        $"Unexpected error occurred setting email for user with ID '{user.Id}'.");
            }

            var phoneNumber = user.PhoneNumber;
            if (vm.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, vm.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                    throw new ApplicationException(
                        $"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
            }

            var firstName = user.FirstName;
            if (vm.FirstName != firstName)
            {
                user.FirstName = vm.FirstName;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new ApplicationException(
                        $"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
            }

            var lastName = user.LastName;
            if (vm.LastName != lastName)
            {
                user.LastName = vm.LastName;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new ApplicationException(
                        $"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
            }

            var country = user.Country;
            if (vm.Country != country)
            {
                user.Country = vm.Country;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new ApplicationException(
                        $"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
            }

            var address = user.Address;
            if (vm.Address != address)
            {
                user.Address = vm.Address;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new ApplicationException(
                        $"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
            }

            var zipcode = user.ZipCode;
            if (vm.ZipCode != zipcode)
            {
                user.ZipCode = vm.ZipCode;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new ApplicationException(
                        $"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
            }

            StatusMessage = "Your profile has been updated";
            return RedirectToAction("UserDetails", new {username = user.UserName});
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UserDelete(string username)
        {
            var asset = _manage.GetRegisteredUsers();

            var model = asset.Select(result => new UserManagementViewModel
            {
                Email = result.Email,
                EmailConfirmed = result.EmailConfirmed,
                Username = result.UserName
            });

            return View(model);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        }

        #endregion
    }
}