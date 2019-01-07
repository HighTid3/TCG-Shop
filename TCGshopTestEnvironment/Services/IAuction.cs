using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCGshopTestEnvironment.ViewModels;

namespace TCGshopTestEnvironment.Services
{
    public interface IAuction
    {
        IEnumerable<AuctionViewModel> GetAuctionCards();

        AuctionDetailViewModel GetByID(int id);
    }
}
