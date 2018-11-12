using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http2;
using TCGshopTestEnvironment.Services;
using TCGshopTestEnvironment.ViewModels;
using X.PagedList.Mvc;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.Models.JoinTables;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net;
using Minio;
using Minio.DataModel;
using Minio.Exceptions;


namespace TCGshopTestEnvironment.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProducts _assets;

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


        public ProductsController(IProducts assets, DBModel context)
        {
            _assets = assets;
            _context = context;
        }

        public async Task<IActionResult> Index(int? page, int? pageAmount, string cardType, string sortBy,
            [FromQuery] List<string> catagorie, [FromQuery] List<string> grades, float? priceLow, float? priceHigh)
        {
            //if page/pageamount/price parameters are empty, fill with standard value
            var pageNumber = page ?? 1;
            var pageAmnt = pageAmount ?? 16;

            float priceL = priceLow ?? 0;
            float priceH = priceHigh ?? 10000;


            //queries to get cards and catagories from database
            var assetModels = _assets.GetbyCardType(cardType);
            List<string> cardscategory = assetModels.SelectMany(x => x.Catnames).Distinct().ToList();

            //viewbags to send to the view
            ViewBag.page = page;
            ViewBag.PageAmount = pageAmount;
            ViewBag.name = "Name";
            ViewBag.totalCategory = cardscategory;
            ViewBag.catagorie = catagorie;
            ViewBag.catagoriestring = "";
            ViewBag.grades = grades;
            ViewBag.PriceLow = priceL;
            ViewBag.PriceHigh = priceH;
            ViewBag.cardType = cardType;

            // sorting list for product sorting
            var sorting = new List<SelectListItem>
            {
                new SelectListItem {Text = "Name A-Z", Value = "name"},
                new SelectListItem {Text = "Name Z-A", Value = "name_desc"},
                new SelectListItem {Text = "Price High-Low", Value = "Price"},
                new SelectListItem {Text = "Price Low-High", Value = "price_desc"}
            };
            ViewBag.Sorting = sorting;
            ViewBag.SelectSort = sortBy ?? "Name A-Z";
            ViewBag.sortBy = sortBy;

            //bind products from query(assetModels) to productviewmodel(listingResult) that is used in the view.
            var listingResult = assetModels
                .Select(result => new ProductsViewModel
                {
                    Id = result.prods.ProductId,
                    Name = result.prods.Name,/*.Length < 20 ? result.prods.Name : result.prods.Name.Substring(0, 15) + "...",*/
                    Price = (decimal)result.prods.Price,
                    ImageUrl = result.prods.ImageUrl,
                    Grade = result.prods.Grade,
                    Stock = result.prods.Stock,
                    CardCatagoryList = result.Catnames
                });

            //filters
            if (catagorie.Count > 0)
            {
                //if statement to make sure when in catagorie if you select same catagorie name it doesn't run any code, for performance
                if (catagorie.Count == 1 && catagorie.Contains(cardType))
                {
                }
                else
                {
                    listingResult = listingResult.Where(x => x.CardCatagoryList.Intersect(catagorie).Any());
                }
            }

            if (priceL > 0 || priceH < 10000)
            {
                listingResult = listingResult.Where(x => x.Price >= (decimal)priceL && x.Price <= (decimal)priceH);
            }

            //viewbag for the view with all the grades in it.
            ViewBag.Grade = listingResult.Select(x => x.Grade).Distinct();

            if (grades.Count > 0 && listingResult.Count(x => grades.Contains(x.Grade)) > 0)
            {
                listingResult = listingResult.Where(x => grades.Contains(x.Grade));
            }

            //sorting
            switch (sortBy)
            {
                case "name_desc":
                    listingResult = listingResult.OrderByDescending(s => s.Name);
                    break;
                case "Price":
                    listingResult = listingResult.OrderByDescending(s => s.Price);
                    break;
                case "price_desc":
                    listingResult = listingResult.OrderBy(s => s.Price);
                    break;
                default:
                    listingResult = listingResult.OrderBy(s => s.Name);
                    break;
            }

            var onePageOfProducts = await listingResult.ToPagedListAsync(pageNumber, pageAmnt);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            if (cardType == "Pokemon")
            {
                return View("~/Views/Products/Pokemon/Pokemon.cshtml");
            }

            if (cardType == "YuGiOh")
            {
                return View("~/Views/Products/YuGiOh/YuGiOh.cshtml");
            }

            if (cardType == "Magic")
            {
                return View("~/Views/Products/Magic/Magic.cshtml");
            }

            return View();
        }

        public IActionResult Detail(int id)
        {
            var asset = _assets.GetByID(id);

            var model = new ProductsDetailModel
            {
                Description = asset.Description,
                Grade = asset.Grade,
                Name = asset.Name,
                Price = asset.Price,
                Stock = asset.Stock,
                ImageUrl = asset.ImageUrl,
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Search(int? page, int? pageAmount, string name, string sortBy,
            [FromQuery] List<string> catagorie, [FromQuery] List<string> grades, float? priceLow, float? priceHigh)
        {
            if (!String.IsNullOrEmpty(name))
            {
                var pageNmber = page ?? 1;
                var pageAmnt = pageAmount ?? 16;

                float priceL = priceLow ?? 0;
                float priceH = priceHigh ?? 10000;


                //queries to get items and catagories from database
                var assetmodel = _assets.GetByNameSearch(name.ToLower());
                List<string> cardscategory = assetmodel.SelectMany(x => x.Catnames).Distinct().ToList();

                //viewbags to send to the view
                ViewBag.page = page;
                ViewBag.PageAmount = pageAmount;
                ViewBag.name = name;
                ViewBag.totalCategory = cardscategory;
                ViewBag.catagorie = catagorie;
                ViewBag.catagoriestring = "";
                ViewBag.grades = grades;
                ViewBag.PriceLow = priceL;
                ViewBag.PriceHigh = priceH;
                ViewBag.cardType = "";

                // sorting list
                List<SelectListItem> Sorting = new List<SelectListItem>
                {
                    new SelectListItem {Text = "Name A-Z", Value = "name"},
                    new SelectListItem {Text = "Name Z-A", Value = "name_desc"},
                    new SelectListItem() {Text = "Price High-Low", Value = "Price"},
                    new SelectListItem() {Text = "Price Low-High", Value = "price_desc"}
                };
                ViewBag.Sorting = Sorting;
                ViewBag.SelectSort = sortBy ?? "Name A-Z";
                ViewBag.sortBy = sortBy;


                // bind all products from database to productviewmodel
                var listingResult = assetmodel
                    .Select(result => new ProductsViewModel
                    {
                        Id = result.prods.ProductId,
                        Name = result.prods.Name,/*.Length < 20 ? result.prods.Name : result.prods.Name.Substring(0, 15) + "...",*/
                        Price = (decimal)result.prods.Price,
                        ImageUrl = result.prods.ImageUrl,
                        Grade = result.prods.Grade,
                        Stock = result.prods.Stock,
                        CardCatagoryList = result.Catnames
                    });


                //filters
                if (catagorie.Count > 0)
                {
                    listingResult = listingResult.Where(x => x.CardCatagoryList.Intersect(catagorie).Any());
                }


                if (priceL > 0 || priceH < 10000)
                {
                    listingResult = listingResult.Where(x => x.Price >= (decimal)priceL && x.Price <= (decimal)priceH);
                }

                //viewbag for the view with all the grades in it.
                ViewBag.Grade = listingResult.Select(x => x.Grade).Distinct();

                if (grades.Count > 0 && listingResult.Count(x => grades.Contains(x.Grade)) > 0)
                {
                    listingResult = listingResult.Where(x => grades.Contains(x.Grade));
                }

                //sorting
                switch (sortBy)
                {
                    case "name_desc":
                        listingResult = listingResult.OrderByDescending(s => s.Name);
                        break;
                    case "Price":
                        listingResult = listingResult.OrderByDescending(s => s.Price);
                        break;
                    case "price_desc":
                        listingResult = listingResult.OrderBy(s => s.Price);
                        break;
                    default:
                        listingResult = listingResult.OrderBy(s => s.Name);
                        break;
                }


                var onePageOfProducts = await listingResult.AsNoTracking().ToPagedListAsync(pageNmber, pageAmnt);
                ViewBag.OnePageOfProducts = onePageOfProducts;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public JsonResult CardAutoCompleteResult(string text)
        {
            IEnumerable<string> cardname = _assets.GetByNameSearch(text).Select(x => x.prods.Name).ToList();

            return Json(cardname);
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
        public async Task<IActionResult> NewProduct(ProductsNewProductViewModel vm)
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
                return Json(new {status = "error", message = "The model is not correct"});
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
    }
}