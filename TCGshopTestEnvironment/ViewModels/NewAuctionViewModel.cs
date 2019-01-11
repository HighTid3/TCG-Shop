using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TCGshopTestEnvironment.Services;
using System.Linq;
using System.Threading.Tasks;


namespace TCGshopTestEnvironment.ViewModels
{
    public class NewAuctionViewModel
    {
        [Required(ErrorMessage ="Please enter the name of the product.")]
        [MaxLength(256)]
        [Display(Name = "Auction Name")]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Please enter the starting price of the auction.")]
        [Display(Name = "Starting Bid")]
        public decimal StartingBid { get; set; }

        [Required(ErrorMessage = "Please enter the end time of the auction.")]
        [Display(Name = "When must the auction end?")]
        [DataType(DataType.Date)]
        [DateValidation("01/01/2500", ErrorMessage ="Auction end time can't be in the past.")]
        public DateTime AuctionEndTime { get; set; }

        [Required(ErrorMessage = "Please enter the grade of the product.")]
        [Range(1, 10, ErrorMessage ="Grade should be between 1 and 10.")]
        [Display(Name = "Card Grade")]
        public string Grade { get; set; }

        public int Stock { get; set; }

        [Required(ErrorMessage = "Please add a category.")]
        public List<string> Category { get; set; }
    }
}
