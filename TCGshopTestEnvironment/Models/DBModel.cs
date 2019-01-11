using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TCGshopTestEnvironment.Models.JoinTables;

namespace TCGshopTestEnvironment.Models
{
    public class DBModel : IdentityDbContext<UserAccount>
    {
        public DBModel(DbContextOptions<DBModel> options) : base(options)
        {
        }

        public DbSet<UserAccount> userAccounts { get; set; }
        public DbSet<Products> products { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Wishlist> wishlists { get; set; }
        public DbSet<Statistics> statistics { get; set; }
        public DbSet<ShoppingBasket> Basket { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductsCat> ProductsCat { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<AuctionBids> AuctionBids { get; set; }
        public DbQuery<StatSumOrder> StatSumOrders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //productcategory jointable
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductCategory>()
                .HasKey(t => new { t.ProductId, t.CategoryName });
            modelBuilder.Entity<ProductCategory>()
                .HasOne(ma => ma.Category)
                .WithMany(m => m.Products)
                .HasForeignKey(ma => ma.CategoryName);
            modelBuilder.Entity<ProductCategory>()
                .HasOne(ma => ma.Products)
                .WithMany(m => m.Category)
                .HasForeignKey(ma => ma.ProductId);

            modelBuilder.Entity<Wishlist>()
                .HasKey(t => new { t.Id });
            modelBuilder.Entity<Wishlist>()
                .HasOne(ma => ma.User)
                .WithMany(m => m.Products)
                .HasForeignKey(ma => ma.UserId);
            modelBuilder.Entity<Wishlist>()
                .HasOne(ma => ma.Product)
                .WithMany(m => m.User)
                .HasForeignKey(ma => ma.ProductId);

            modelBuilder.Entity<AuctionBids>()
                .HasKey(t => new {t.Id});
            modelBuilder.Entity<AuctionBids>()
                .HasOne(ma => ma.User)
                .WithMany(m => m.AuctionProducts)
                .HasForeignKey(ma => ma.UserId);
            modelBuilder.Entity<AuctionBids>()
                .HasOne(ma => ma.Product)
                .WithMany(m => m.AuctionUser)
                .HasForeignKey(ma => ma.ProductId);
        }
    }
}