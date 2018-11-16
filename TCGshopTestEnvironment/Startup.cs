using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.Services;

namespace TCGshopTestEnvironment
{
    public class Startup
    {
        //S3

        public static string accessKey = Environment.GetEnvironmentVariable("accessKey");
        public static string secretKey = Environment.GetEnvironmentVariable("secretKey");
        public static string s3Server = Environment.GetEnvironmentVariable("s3Server");
        //S3

        public static string storagePath = "https://cdn.tcg.sale/tcg-upload/";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddIdentity<UserAccount, IdentityRole>()
                .AddEntityFrameworkStores<DBModel>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                // Email Settings
                options.SignIn.RequireConfirmedEmail = true;

            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton(Configuration);
            services.AddScoped<IProducts, ProductService>();
            services.AddScoped<IShopping, ShoppingService>();
            services.AddScoped<IWishlist, WishlistService>();


            var connection = @"User ID=veecnnbwltunor;Password=a6c8cd6bfb0cb30915f3980c7a83949232ba2051cfc06c894cae20d539f452bf;Host=ec2-54-247-79-32.eu-west-1.compute.amazonaws.com;Port=5432;Database=d4phdsktctlcgc;Use SSL Stream=True;SSL Mode=Require;TrustServerCertificate=True;";

            
            // Heroku provides PostgreSQL connection URL via env variable
            var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            var connStr = "";

            if (connUrl == null)
            {
                connStr = connection;
            }
            else
            {

                // Parse connection URL to connection string for Npgsql
                connUrl = connUrl.Replace("postgres://", string.Empty);

                var pgUserPass = connUrl.Split("@")[0];
                var pgHostPortDb = connUrl.Split("@")[1];
                var pgHostPort = pgHostPortDb.Split("/")[0];

                var pgDb = pgHostPortDb.Split("/")[1];
                var pgUser = pgUserPass.Split(":")[0];
                var pgPass = pgUserPass.Split(":")[1];
                var pgHost = pgHostPort.Split(":")[0];
                var pgPort = pgHostPort.Split(":")[1];

                connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};Use SSL Stream=True;SSL Mode=Require;TrustServerCertificate=True;";
            }
            
            services.AddDbContext<DBModel>(options => options.UseNpgsql(connStr));


            services.AddSingleton<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            //sessions configuration for shopping basket
            services.AddDistributedMemoryCache();
            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
