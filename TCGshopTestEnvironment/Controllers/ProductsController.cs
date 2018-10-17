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
namespace TCGshopTestEnvironment.Controllers
{
    public class ProductsController : Controller
    {
        private IProducts _assets;

        public ProductsController(IProducts assets)
        {
            _assets = assets;
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
                    Name = result.Name.Length < 20 ? result.Name : result.Name.Substring(0, 20) + "...",
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
        public IActionResult Search(int? page, int? pagAmount, string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                ViewBag.page = page;
                ViewBag.PageAmount = pagAmount;
                ViewBag.name = name;
                var pageNmber = page ?? 1;
                var pageAmnt = pagAmount ?? 10;
                var assetmodel = _assets.GetByNameSearch(name.ToLower());
                var cardscategory = _assets.GetCardCatagory(assetmodel);
                ViewBag.Category = cardscategory;
                var listingResult = assetmodel
                    .Select(result => new ProductsViewModel
                    {
                        Id = result.ProductId,
                        Name = result.Name,
                        Price = result.Price,
                        ImageUrl = result.ImageUrl,
                        Grade = result.Grade,
                        Stock = result.Stock
                    });

                var onePageOfProducts = listingResult.ToPagedList(pageNmber, pageAmnt);
                ViewBag.OnePageOfProducts = onePageOfProducts;

                return View(listingResult);
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

    }
}

