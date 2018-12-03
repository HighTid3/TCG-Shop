namespace TCGshopTestEnvironment.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual Products Product { get; set; }
        public virtual Order Order { get; set; }
    }
}