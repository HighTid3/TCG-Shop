using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.ViewModels;

namespace TCGshopTestEnvironment.Services
{
    public class ProductService : IProducts
    {
        private readonly DBModel _context;

        public ProductService(DBModel context)
        {
            _context = context;
        }

        public void Add(Products NewProduct)
        {
            _context.Add(NewProduct);
            _context.SaveChanges();
        }

        public ProductsDetailModel GetByID(int id)
        {
            return (from p in _context.products
                    let categories = (from c in _context.ProductCategory
                                      where c.ProductId == id
                                      select c.CategoryName).ToList()
                    where p.ProductId == id
                    select new ProductsDetailModel
                    {
                        CardCatagoryList = categories,
                        Description = p.Description,
                        Grade = p.Grade,
                        Id = p.ProductId,
                        ImageUrl = p.ImageUrl,
                        Name = p.Name,
                        Price = p.Price,
                        Stock = p.Stock
                    }).FirstOrDefault();
        }

        public Products GetProductsById(int id)
        {
            return _context.products
                .FirstOrDefault(product => product.ProductId == id);
        }

        public IEnumerable<Productsandcategorie> GetbyCardType(string type)
        {
            IEnumerable<ProductsCat> results = _context.ProductsCat.FromSql(
                "SELECT products.*, string_agg(\"CategoryName\", \',\') as CategoryName " +
                "FROM products LEFT JOIN \"ProductCategory\" ON products.\"ProductId\" = \"ProductCategory\".\"ProductId\" " +
                "GROUP BY products.\"ProductId\"").ToArray();

            var ProductsAndCategories = new List<Productsandcategorie>();

            foreach (var ProductsCat in results)
            {
                var CatNames = new List<string>();

                try
                {
                    CatNames = ProductsCat.CategoryName.Split(',').ToList();
                }
                catch (Exception e)
                {
                    CatNames = new List<string> { "" };
                }

                if (type != "All")
                {
                    if (CatNames.Contains(type))
                        ProductsAndCategories.Add(new Productsandcategorie
                        {
                            prods = ProductsCat,
                            Catnames = CatNames
                        });
                }
                else
                {
                    ProductsAndCategories.Add(new Productsandcategorie
                    {
                        prods = ProductsCat,
                        Catnames = CatNames
                    });
                }
            }

            return ProductsAndCategories;
        }

        //(type != "Default")
        //return from p in _context.products
        //       join c in _context.ProductCategory on p.ProductId equals c.ProductId
        //       let categorienames = (from d in _context.ProductCategory
        //                             where p.ProductId == d.ProductId && d.CategoryName == type
        //                             select d.CategoryName).ToList()
        //       where c.CategoryName == type
        //       select new Productsandcategorie { prods = p, Catnames = categorienames };
        //}
        //else
        //{
        //    return from p in _context.products
        //            let categorienames = (from d in _context.ProductCategory
        //                                 where p.ProductId == d.ProductId
        //                                 select d.CategoryName).ToList()
        //           select new Productsandcategorie { prods = p, Catnames = categorienames };
        //}

        public IEnumerable<Productsandcategorie> GetByNameSearch(string name)
        {
            IEnumerable<ProductsCat> results = _context.ProductsCat.FromSql(
                "SELECT products.*, string_agg(\"CategoryName\", \',\') as CategoryName " +
                "FROM products LEFT JOIN \"ProductCategory\" ON products.\"ProductId\" = \"ProductCategory\".\"ProductId\" " +
                "GROUP BY products.\"ProductId\"").ToArray();

            var ProductsAndCategories = new List<Productsandcategorie>();

            foreach (var ProductsCat in results)
                if (ProductsCat.Name.ToLower().Contains(name.ToLower()) || ProductsCat.Name.ToLower() == name.ToLower())
                {
                    var CatNames = new List<string>();
                    try
                    {
                        CatNames = ProductsCat.CategoryName.Split(',').ToList();
                    }
                    catch (Exception e)
                    {
                        CatNames = new List<string> { "" };
                    }

                    ProductsAndCategories.Add(new Productsandcategorie { prods = ProductsCat, Catnames = CatNames });
                }

            return ProductsAndCategories;

            //      return from p in _context.products
            //       where p.Name.ToLower() == name || p.Name.ToLower().Contains(name)
            //             let categorienames = (from d in _context.ProductCategory
            //                                  where p.ProductId == d.ProductId
            //                                  select d.CategoryName).ToList()

            //    select new Productsandcategorie { prods = p, Catnames = categorienames };
        }

        public IEnumerable<PopularViewModel> GetMostViewed()
        {
            var result = (from p in _context.products
                          where p.ViewsDetails > 10
                          orderby p.ViewsDetails descending
                          select new PopularViewModel
                          {
                              ProductId = p.ProductId,
                              Name = p.Name,
                              Price = p.Price,
                              DateCreated = p.DateCreated,
                              DateUpdated = p.DateUpdated,
                              ViewsListed = p.ViewsListed,
                              ViewsDetails = p.ViewsDetails,
                              ImageUrl = p.ImageUrl
                          }).Take(6).ToList();

            return result;
        }
    }
}