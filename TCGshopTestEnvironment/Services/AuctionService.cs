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
    }
}
