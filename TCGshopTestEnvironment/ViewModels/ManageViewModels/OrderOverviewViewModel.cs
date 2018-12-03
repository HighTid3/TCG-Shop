using System;

namespace TCGshopTestEnvironment.ViewModels.ManageViewModels
{
    public class OrderOverviewViewModel
    {
        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }

        public string Ordernr { get; set; }

        public int OrderId { get; set; }
        public string StatusMessage { get; set; }
    }
}