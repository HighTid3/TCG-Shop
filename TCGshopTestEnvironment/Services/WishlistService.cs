using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.ViewModels;

namespace TCGshopTestEnvironment.Services
{
    public class WishlistService : IWishlist
    {
        private readonly DBModel _context;
        public WishlistService(DBModel context)
        {
            _context = context;
        }
        public IQueryable<Wishlist> WishlistByUserid(string userid)
        {
            return from s in _context.wishlists
                where s.UserId == userid
                select new Wishlist {  Id = s.Id, ProductId = s.ProductId, UserId = s.UserId, User = s.User, Product = s.Product};
        }

        public IQueryable<WishlistViewModel> WishlistItems(string userid)
        {
            return from p in _context.products
                from b in _context.wishlists
                where b.UserId == userid && p.ProductId == b.ProductId
                select new WishlistViewModel
                {
                    WishlistId = b.Id,
                    Id = b.ProductId,
                    ImageUrl = p.ImageUrl,
                    Name = p.Name,
                    Price = (decimal)p.Price,
                    Grade = p.Grade,

                };

        }
    }
}
