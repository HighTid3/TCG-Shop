using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.ViewModels.ManageViewModels;

namespace TCGshopTestEnvironment.Services
{
    public interface IManage
    {
        IEnumerable<OrderOverviewViewModel> OrderOverview(string useremail);

        OrderDetailsViewModel Orderdetails(string useremail, int OrderId);

        IEnumerable<UserAccount> GetRegisteredUsers();

        UserAccount GetRegisteredUserbyUsername(string username);
    }


}
