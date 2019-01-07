using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.ViewModels;

namespace TCGshopTestEnvironment.Services
{
    public class AuctionService : IAuction
    {
        private static DBModel _context;

        public AuctionService(DBModel context)
        {
            _context = context;

        }

        public IEnumerable<AuctionViewModel> GetAuctionCards()
        {
            return (from p in _context.products
                join c in _context.ProductCategory on p.ProductId equals c.ProductId
                where c.CategoryName == "Auction"
                select new AuctionViewModel
                {
                    AuctionEnd = p.AuctionEndTime,
                    AuctionStart = p.DateCreated,
                    Id = p.ProductId,
                    Grade = p.Grade,
                    ImageUrl = p.ImageUrl,
                    Name = p.Name,

                }).ToList();

        }

        public AuctionDetailViewModel GetByID(int id)
        {
            var HighestBid = _context.AuctionBids.Where(x => x.ProductId == id).Select(x => x.Bid).DefaultIfEmpty().Max();
            return (from p in _context.products
                let AuctionBids = (from a in _context.AuctionBids
                    where a.Product.ProductId == p.ProductId

                    select new AuctionBids
                    {
                        Bid = a.Bid,
                        User = a.User,
                        BidDate = a.BidDate,
                        Id = a.Id,
                        Product = a.Product,
                        ProductId = a.ProductId,
                        UserId = a.UserId

                    }).OrderByDescending(x => x.Bid).Take(5).ToList()
                where p.ProductId == id
                select new AuctionDetailViewModel
                {
                    Id = p.ProductId,
                    AuctionBids = AuctionBids,
                    AuctionEnd = p.AuctionEndTime,
                    AuctionStart = p.DateCreated,
                    Grade = p.Grade,
                    ImageUrl = p.ImageUrl,
                    Name = p.Name,
                    Price = p.Price,
                    HighestBid = HighestBid
        }).FirstOrDefault();
        }
    }
}
