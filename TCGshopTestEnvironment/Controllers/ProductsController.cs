using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TCGshopTestEnvironment.Services;
using TCGshopTestEnvironment.ViewModels;

namespace TCGshopTestEnvironment.Controllers
{
    public class ProductsController : Controller
    {
        private IProducts _assets;

        public ProductsController(IProducts assets)
        {
            _assets = assets;
        }

        public IActionResult Index()
        {
            var assetModels = _assets.GetAll();

            var listingResult = assetModels
                .Select(result => new ProductsViewModel
                {
                    Id = result.ProductID,
                    Name = result.Name,
                    Price = result.Price,
                    ImageUrl = result.ImageUrl
                    

                });
            var model = new ProductsIndexModel()
            {
                Assets = listingResult
            }
            ;
            return View(model);
        }
    }
}
