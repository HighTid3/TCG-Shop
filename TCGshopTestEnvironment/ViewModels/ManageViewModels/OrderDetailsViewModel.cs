using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCGshopTestEnvironment.ViewModels.ManageViewModels
{
    public class OrderDetailsViewModel
    {

        public string Ordernr { get; set; }

        public string Orderstatus { get; set; }
        public System.DateTime OrderDate { get; set; }


        public string FirstName { get; set; }


        public string LastName { get; set; }


        public string Address { get; set; }


        public string City { get; set; }


        public string State { get; set; }


        public string PostalCode { get; set; }


        public string Country { get; set; }


        public string Email { get; set; }

        public decimal Total { get; set; }

        public List<OrderViewModel> Orderdetails { get; set; }
    }
}
