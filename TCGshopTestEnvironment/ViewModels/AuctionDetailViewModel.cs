using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TCGshopTestEnvironment.Models;

namespace TCGshopTestEnvironment.ViewModels
{
    public class AuctionDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Grade { get; set; }
        public DateTime AuctionStart { get; set; }
        public DateTime AuctionEnd { get; set; }

        public decimal Price { get; set; }

        public List<AuctionBids> AuctionBids { get; set; }

        public decimal HighestBid { get; set; }
    }
}
