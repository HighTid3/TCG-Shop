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
                    ImageUrl = result.ImageUrl,
                    Grade = result.Grade,
                    Stock = result.Stock
                    
                    

                });
            var model = new ProductsIndexModel()
            {
                Assets = listingResult
            }
            ;
            return View(model);
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
        }
    }

