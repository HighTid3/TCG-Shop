using System.Collections.Generic;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.ViewModels.ManageViewModels;

namespace TCGshopTestEnvironment.Services
{
    public interface IManage
    {
        IEnumerable<OrderOverviewViewModel> OrderOverview(string useremail);

        IEnumerable<OrderOverviewViewModel> GetAllOrders();

        OrderDetailsViewModel Orderdetails(int OrderId);

        IEnumerable<UserAccount> GetRegisteredUsers();

        UserAccount GetRegisteredUserbyUsername(string username);

        IEnumerable<Category> GetAllCategories();
    }
}