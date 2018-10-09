﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IActionResult Index(int? page, int? pageAmount)
        {
            ViewBag.page = page;
            ViewBag.PageAmount = pageAmount;
            var assetModels = _assets.GetAll();
            var pageNumber = page ?? 1;
            var pageAmnt = pageAmount ?? 10;
            var listingResult = assetModels
                .Select(result => new ProductsViewModel
                {
                    Id = result.ProductId,
                    Name = result.Name,
                    Price = result.Price,
                    ImageUrl = result.ImageUrl,
                    Grade = result.Grade,
                    Stock = result.Stock

                });
            var model = new ProductsIndexModel()
            {
                Assets = listingResult
            };
            var onePageOfProducts = listingResult.ToPagedList(pageNumber, pageAmnt);
            ViewBag.OnePageOfProducts = onePageOfProducts;

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
        }
    }

