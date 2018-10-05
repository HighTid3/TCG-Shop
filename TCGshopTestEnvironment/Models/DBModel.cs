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
        public DBModel(DbContextOptions<DBModel> options) : base(options) { 

        }
        public DbSet<UserAccount> userAccounts { get; set; }
        public DbSet<Products> products { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Whishlist> whishlists  { get; set; }
        public DbSet<Orders> orders { get; set; }
        public DbSet<Statistics> statistics { get; set; }
        public DbSet<Pictures> pictures { get; set; }

        public DbSet<ProductCategory> ProductCategory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        }

    }


}
