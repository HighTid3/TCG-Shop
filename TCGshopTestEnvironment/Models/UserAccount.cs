using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TCGshopTestEnvironment.Models
{
    public class UserAccount : IdentityUser
    {
        public string Country { get; set; }

        public string Address { get; set; }

        public string ZipCode { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }

        public virtual List<Wishlist> Products { get; set; }

        public virtual List<AuctionBids> AuctionProducts { get; set; }
    }
}