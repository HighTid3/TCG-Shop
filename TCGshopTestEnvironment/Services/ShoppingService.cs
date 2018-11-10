using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.Models.JoinTables;
using TCGshopTestEnvironment.ViewModels;

namespace TCGshopTestEnvironment.Services
{
    public class ShoppingService : IShopping
    {
        private readonly DBModel _context;
        public ShoppingService(DBModel context)
        {
            _context = context;
        }
        public IQueryable<ShoppingBasket> ShoppingbasketByName(string name)
        {
            return from s in _context.Basket
                where s.UserId == name
                select new ShoppingBasket {Amount = s.Amount, Id = s.Id, ProductsId = s.ProductsId, UserId = s.UserId};
        }

        public IQueryable<ProductsShopCartViewModel> ShoppinCartItems(string name)
        {
            return from p in _context.products
                from b in _context.Basket
                where b.UserId == name && p.ProductId == b.ProductsId
                select new ProductsShopCartViewModel
                {
                    Amount = b.Amount,
                    ProductId = b.ProductsId,
                    ImageUrl = p.ImageUrl,
                    Name = p.Name,
                    Price = p.Price,
                    Grade = p.Grade,
                };

        }
    }
}
