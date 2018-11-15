using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public DbSet<Orders> orders { get; set; }
        public DbSet<Statistics> statistics { get; set; }
        public DbSet<Pictures> pictures { get; set; }

        public DbSet<ShoppingBasket> Basket { get; set; }

        public DbSet<ProductCategory> ProductCategory { get; set; }

        public DbSet<ProductsCat> ProductsCat { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //productcategory jointable
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductCategory>()
                .HasKey(t => new {t.ProductId, t.CategoryName});
            modelBuilder.Entity<ProductCategory>()
                .HasOne(ma => ma.Category)
                .WithMany(m => m.Products)
                .HasForeignKey(ma => ma.CategoryName);
            modelBuilder.Entity<ProductCategory>()
                .HasOne(ma => ma.Products)
                .WithMany(m => m.Category)
                .HasForeignKey(ma => ma.ProductId);

            modelBuilder.Entity<Wishlist>()
                .HasKey(t => new { t.Id});
            modelBuilder.Entity<Wishlist>()
                .HasOne(ma => ma.User)
                .WithMany(m => m.Products)
                .HasForeignKey(ma => ma.UserId);
            modelBuilder.Entity<Wishlist>()
                .HasOne(ma => ma.Product)
                .WithMany(m => m.User)
                .HasForeignKey(ma => ma.ProductId);


        }
    }
}


