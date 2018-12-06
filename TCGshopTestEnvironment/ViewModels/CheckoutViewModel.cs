using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCGshopTestEnvironment.ViewModels
{
    public class CheckoutViewModel
    {
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
