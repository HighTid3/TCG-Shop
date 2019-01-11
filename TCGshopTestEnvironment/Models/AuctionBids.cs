using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCGshopTestEnvironment.Models
{
    public class AuctionBids
    {
        [Key]
        public int Id { get; set; }

        public decimal Bid { get; set; }

        public DateTime BidDate { get; set; }

        public string UserId { get; set; }
        public UserAccount User { get; set; }

        public int ProductId { get; set; }
        public Products Product { get; set; }
    }
}
