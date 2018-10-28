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

namespace TCGshopTestEnvironment.Controllers
{
    public class ProductsController : Controller
    {
        private IProducts _assets;
        private DBModel dbModel;

        private DBModel _context;


        public ProductsController(IProducts assets, DBModel context)
        {
            _assets = assets;
            _context = context;
        }

        public IActionResult Index(int? page, int? pageAmount, string cardType)
        {
            ViewBag.page = page;
            ViewBag.PageAmount = pageAmount;
            ViewBag.CardType = cardType;
            //var assetModels = _assets.GetAll();
            var assetModels = _assets.GetbyCardType(cardType);
            if (cardType == "Default")
            {
                assetModels = _assets.GetAll();
            }
            var pageNumber = page ?? 1;
            var pageAmnt = pageAmount ?? 8;
            var listingResult = assetModels
                .Select(result => new ProductsViewModel
                {
                    Id = result.ProductId,
                    Name = result.Name.Length < 20 ? result.Name : result.Name.Substring(0, 15) + "...",
                    Price = result.Price,
                    ImageUrl = result.ImageUrl,
                    Grade = result.Grade,
                    Stock = result.Stock

                });
            //var model = new ProductsIndexModel()
            //{
            //    Assets = listingResult
            //};
            var onePageOfProducts = listingResult.ToPagedList(pageNumber, pageAmnt);
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
        public IActionResult Search(int? page, int? pageAmount, string name, string sortBy, [FromQuery] List<string> catagorie, [FromQuery] List<string> grades)
        {
            if (!String.IsNullOrEmpty(name))
            {
                var pageNmber = page ?? 1;
                var pageAmnt = pageAmount ?? 10;

                //queries to get items and catagories from database
                var assetmodel = _assets.GetByNameSearch(name.ToLower());
                var cardscategory = _assets.GetCardCatagory(assetmodel);

                //viewbags to send to the view
                ViewBag.page = page;
                ViewBag.PageAmount = pageAmount;
                ViewBag.name = name;
                ViewBag.totalCategory = cardscategory;
                ViewBag.catagorie = catagorie;
                ViewBag.catagoriestring = "";
                ViewBag.grades = grades;
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
                        Id = result.ProductId,
                        Name = result.Name,
                        Price = result.Price,
                        ImageUrl = result.ImageUrl,
                        Grade = result.Grade,
                        Stock = result.Stock,
                        CardCatagoryList = _context.ProductCategory.Where(x => x.ProductId == result.ProductId).Select(x => x.CategoryName).ToList()
                        
                    });

                
                //filters
                //if(!String.IsNullOrEmpty(catagorie)) listingResult = listingResult.Where(x => x.CardCatagoryList.Contains(catagorie));
                if (catagorie.Count > 0)
                {
                    listingResult = listingResult.Where(x => x.CardCatagoryList.Intersect(catagorie).Any());
                }

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


                var onePageOfProducts = listingResult.ToPagedList(pageNmber, pageAmnt);
                ViewBag.OnePageOfProducts = onePageOfProducts;
                return View();
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }

        public JsonResult CardAutoCompleteResult(string text)
        {

            IEnumerable<string> cardname = _assets.GetByNameSearch(text).Select(x => x.Name).ToList();

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

    }
}

