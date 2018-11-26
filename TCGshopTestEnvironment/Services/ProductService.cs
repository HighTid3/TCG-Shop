using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.ViewModels;
using X.PagedList;

namespace TCGshopTestEnvironment.Services
{
    public class ProductService : IProducts
    {
        private DBModel _context;

        public ProductService(DBModel context)
        {
            _context = context;
        }

        public void Add(Products NewProduct)
        {
            _context.Add(NewProduct);
            _context.SaveChanges();

        }

        public Products GetByID(int id)
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

            List<Productsandcategorie> ProductsAndCategories = new List<Productsandcategorie>();

            foreach (var ProductsCat in results)
            {

                List<string> CatNames = new List<string>();

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
                    {

                        ProductsAndCategories.Add(new Productsandcategorie
                        {
                            prods = ProductsCat,
                            Catnames = CatNames
                        });
                    }
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

            List<Productsandcategorie> ProductsAndCategories = new List<Productsandcategorie>();


            foreach (var ProductsCat in results)
            {
                if (ProductsCat.Name.ToLower().Contains(name.ToLower()) || ProductsCat.Name.ToLower() == name.ToLower())
                {
                    List<string> CatNames = new List<string>();
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
                          }).ToList();
            return result;
        } 
    }
}
