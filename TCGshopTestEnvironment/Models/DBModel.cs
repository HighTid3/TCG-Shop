using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace TCGshopTestEnvironment.Models
{
    public class DBModel : IdentityDbContext<UserAccount>
    {
        public DBModel(DbContextOptions<DBModel> options) : base(options) { 

        }
        public DbSet<UserAccount> userAccounts { get; set; }
        public DbSet<Products> products { get; set; }
        public DbSet<Catagory> catagories { get; set; }
        public DbSet<Whishlist> whishlists  { get; set; }
        public DbSet<Orders> orders { get; set; }
        public DbSet<Statistics> statistics { get; set; }
        public DbSet<Pictures> pictures { get; set; }

    }
}
