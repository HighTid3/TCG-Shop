namespace TCGshopTestEnvironment.Models.JoinTables
{
    public class ProductCategory
    {
        public int ProductId { get; set; }
        public Products Products { get; set; }
        public string CategoryName { get; set; }
        public Category Category { get; set; }
    }
}