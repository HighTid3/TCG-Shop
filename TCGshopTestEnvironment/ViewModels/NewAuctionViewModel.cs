using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCGshopTestEnvironment.ViewModels
{
    public class NewAuctionViewModel
    {
        [Display(Name = "Auction Name")]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        [Display(Name = "Starting Bid")]
        public decimal StartingBid { get; set; }
        [Display(Name = "When must the auction end?")]
        public DateTime AuctionEndTime { get; set; }
        [Display(Name = "Card Grade")]
        public string Grade { get; set; }
        public int Stock { get; set; }
        public List<string> Category { get; set; }
    }
}
