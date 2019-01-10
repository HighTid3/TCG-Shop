using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Minio;
using Minio.Exceptions;
using NuGet.Frameworks;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.Models.JoinTables;
using TCGshopTestEnvironment.Services;
using TCGshopTestEnvironment.ViewModels;
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
        private readonly DBModel _context;

        //S3
        //public static AmazonS3Config config = new AmazonS3Config
        //{
        //    RegionEndpoint = RegionEndpoint.USEast1, // MUST set this before setting ServiceURL and it should match the `MINIO_REGION` enviroment variable.
        //    ServiceURL = "http://10.0.0.10:9000", // replace http://localhost:9000 with URL of your minio server
        //    ForcePathStyle = true // MUST be true to work correctly with Minio server
        //};
        //public static AmazonS3Client amazonS3Client = new AmazonS3Client(Startup.accessKey, Startup.secretKey, config);
        //S3

        //Minio
        // Initialize the client with access credentials.
        private static MinioClient minio = new MinioClient(Startup.s3Server, Startup.accessKey, Startup.secretKey).WithSSL();

        //Minio

        public ManageController(
            UserManager<UserAccount> userManager,
            ILogger<ManageController> logger,
            SignInManager<UserAccount> signInManager,
            IEmailSender emailSender,
            IManage manage,
            DBModel context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _manage = manage;
            _context = context;
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

            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
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
            if (User.IsInRole("Admin"))
            {
                model = _manage.GetAllOrders();
            }
            // sorting list for product sorting
            var OrderStatuslist = new List<SelectListItem>
            {
                new SelectListItem {Text = "Paid", Value = "paid"},
                new SelectListItem {Text = "Created", Value = "created"},
                new SelectListItem {Text = "Cancelled", Value = "Canceled"},
                new SelectListItem {Text = "Expired", Value = "Expired"},
                new SelectListItem {Text = "Shipped", Value = "Shipped"},
                new SelectListItem {Text = "Completed", Value = "Completed"},

            };
            ViewBag.OrderStatus = OrderStatuslist;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetails(int orderid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var model = _manage.Orderdetails(orderid);
            if (user.Email != model.Email && !User.IsInRole("Admin"))
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UserManagement()
        {
            var asset = _manage.GetRegisteredUsers();

            var model = asset.Select(result => new UserManagementViewModel
            {
                Email = result.Email,
                EmailConfirmed = result.EmailConfirmed,
                Username = result.UserName,
                Name = $"{result.FirstName} {result.LastName}"
            });
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UserDetails(string username)
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
        public IActionResult UserEdit(string username)
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
            return RedirectToAction("UserDetails", new { username = user.UserName });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UserDelete(string username)
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
        public async Task<IActionResult> UserDelete(UserManagementViewModel vm)
        {
            var user = _manage.GetRegisteredUserbyUsername(vm.Username);
            var currentuser = await _userManager.GetUserAsync(User);
            if (user != currentuser)
            {
                _context.userAccounts.Remove(user);
                _context.SaveChanges();
            }

            return RedirectToAction("UserManagement");
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> FixDatabase(bool yes)
        {
            if (yes && string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ProductionTCG")))
            {
                //Delete DB
                await _context.Database.EnsureDeletedAsync();

                //Run Migrations
                _context.Database.Migrate();

                //Load Data in DB
                string categories = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/DbRestore/_categories.sql");
                string products = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/DbRestore/_products.sql");
                string productsCategories = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/DbRestore/_ProductCategory.sql");

                //Dont await any of them, so they all execute async
                await _context.Database.ExecuteSqlCommandAsync(categories);
                await _context.Database.ExecuteSqlCommandAsync(products);
                await _context.Database.ExecuteSqlCommandAsync(productsCategories);
            }

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult ChangeorderStatus(string orderstatus, int orderid)
        {
            var model = _context.Orders.Single(x => x.OrderId == orderid);
            model.PaymentStatus = orderstatus;
            _context.Orders.Update(model);
            _context.SaveChanges();
            return Json(new { success = true });
        }

        //Adding New product
        [HttpGet]
        public IActionResult NewProduct()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetCategoryAll()
        {
            IEnumerable<string> categories = _context.categories.Select(x => x.CategoryName).ToList();
            return Json(categories);
        }

        [HttpPost]
        public IActionResult NewProduct(ProductsNewProductViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Products Product = new Products
                {
                    Name = vm.Name,
                    ImageUrl = vm.ImageUrl,
                    Price = vm.Price,
                    Grade = vm.Grade,
                    Stock = vm.Stock,
                };
                _context.Add(Product);

                IEnumerable<string> categories = _context.categories.Select(x => x.CategoryName).ToList();
                foreach (string TestCategory in vm.Category)
                {
                    if (categories.Contains(TestCategory))
                    {
                        Console.WriteLine("Category: " + TestCategory + "is in database");
                    }
                    else
                    {
                        Console.WriteLine("Category: " + TestCategory + " is NOT in database, ADDING!");

                        //Here code to add new category to database
                        Category category = new Category
                        {
                            CategoryName = TestCategory,
                            Description = "NULL"
                        };
                        _context.Add(category);
                    }

                    //Adding date to merge Table
                    ProductCategory productCategory = new ProductCategory
                    {
                        ProductId = Product.ProductId,
                        CategoryName = TestCategory
                    };
                    _context.Add(productCategory);
                }

                _context.SaveChanges();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(FileUpload formFile)
        {
            // Perform an initial check to catch FileUpload class attribute violations.
            if (!ModelState.IsValid)

            {
                return Json(new { status = "error", message = "The model is not correct" });
            }

            //Check MIME
            if (formFile.CardImageUpload.ContentType.ToLower() != "image/png")
                return Json(new
                {
                    status = "error",
                    message = "The file mime must be image/png"
                });

            //Check if size is between 0 and 10MB
            if (formFile.CardImageUpload.Length == 0)
            {
                return Json(new
                {
                    status = "error",
                    message = "Upload Failed. The selected file is empty."
                });
            }
            else if (formFile.CardImageUpload.Length > 10485760)
            {
                return Json(new
                {
                    status = "error",
                    message = "The selected file exceeds 10 MB."
                });
            }

            //Generate Random Name
            //string ext = System.IO.Path.GetExtension(formFile.CardImageUpload.FileName); //Get the file extension

            string objectName = Guid.NewGuid() + ".png";

            var filePath = System.IO.Path.GetTempFileName() + objectName; //Create Temp File with Random GUID

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CardImageUpload.CopyToAsync(fileStream);
            }

            //Prepare S3 Upload
            var bucketName = "tcg-upload";
            var location = "us-east-1";
            var contentType = formFile.CardImageUpload.ContentType.ToLower();

            try
            {
                // Make a bucket on the server, if not already present.
                bool found = await minio.BucketExistsAsync(bucketName);
                if (!found)
                {
                    await minio.MakeBucketAsync(bucketName, location);
                }

                // Upload a file to bucket.
                await minio.PutObjectAsync(bucketName, objectName, filePath, contentType);
                Console.Out.WriteLine("Successfully uploaded " + objectName);

                return Json(new
                {
                    status = "Ok",
                    message = "Successfully uploaded " + objectName,
                    image = objectName
                });
            }
            catch (MinioException e)
            {
                return Json(new
                {
                    status = "Error",
                    message = "File Upload Error:" + e.Message
                });
            }
        }

        [HttpGet]
        public IActionResult FileUpload()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ManageCategories()
        {
            var asset = _manage.GetAllCategories();

            var model = asset.Select(result => new CategoryViewModel
            {
                CategoryName = result.CategoryName,
                Description = result.Description
            });
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CategoryEdit(string categoryname, string categorydescription)
        {
            var asset = _context.categories.Single(x => x.CategoryName == categoryname);
            asset.Description = categorydescription;
            _context.categories.Update(asset);
            _context.SaveChanges();
            return Json(new { success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CategoryDelete(string CategoryName)
        {
            var asset = _context.categories.Single(x => x.CategoryName == CategoryName);
            var model = new CategoryViewModel
            {
                CategoryName = asset.CategoryName,
                Description = asset.Description
            };
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CategoryDelete(CategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _context.categories.RemoveRange(_context.categories.Where(x => x.CategoryName == vm.CategoryName));
                _context.ProductCategory.RemoveRange(_context.ProductCategory.Where(x => x.CategoryName == vm.CategoryName));
                _context.SaveChanges();
            }
               
            return RedirectToAction("ManageCategories");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CategoryAdd()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CategoryAdd(CategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var asset = new Category
                {
                    CategoryName = vm.CategoryName,
                    Description = vm.Description
                };
                _context.categories.Add(asset);
                _context.SaveChanges();
            }
            else
            {
                return View();
            }
            return RedirectToAction("ManageCategories");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Statistics()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ChartGenerator()
        {
            return PartialView();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult ChartGenerator(ChartViewModel ChartViewModel)
        {

            return PartialView();
        }







        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        }

        #endregion Helpers
    }
}