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
        public IQueryable<ShoppingBasket> ShoppingbasketByName(string userid)
        {
            return from s in _context.Basket
                where s.UserId == userid
                select new ShoppingBasket {Amount = s.Amount, Id = s.Id, ProductsId = s.ProductsId, UserId = s.UserId};
        }

        public IQueryable<ProductsShopCartViewModel> ShoppinCartItems(string userid)
        {
            return from p in _context.products
                from b in _context.Basket
                where b.UserId == userid && p.ProductId == b.ProductsId
                select new ProductsShopCartViewModel
                {
                    CartID = b.Id,
                    Amount = b.Amount,
                    ProductId = b.ProductsId,
                    ImageUrl = p.ImageUrl,
                    Name = p.Name,
                    Price = p.Price,
                    Grade = p.Grade,
                    TotalPrice = Math.Round((p.Price * b.Amount),2, MidpointRounding.AwayFromZero)
                };

        }

        public int CreateOrder(Order order, List<ProductsShopCartViewModel> cartItems)
        {
            decimal orderTotal = 0;

            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Price,
                    Quantity = item.Amount
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Amount * item.Price);

                _context.OrderDetails.Add(orderDetail);
            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Save the order
            _context.SaveChanges();
            // Empty the shopping cart
            //TODO

            // Return the OrderId as the confirmation number
            return order.OrderId;

        }

    }
}
