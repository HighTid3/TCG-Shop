namespace TCGshopTestEnvironment.ViewModels.ManageViewModels
{
    public class OrderViewModel
    {
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string Grade { get; set; }
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }
}