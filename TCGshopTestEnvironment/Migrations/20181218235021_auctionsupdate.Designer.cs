﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TCGshopTestEnvironment.Models;

namespace TCGshopTestEnvironment.Migrations
{
    [DbContext(typeof(DBModel))]
    [Migration("20181218235021_auctionsupdate")]
    partial class auctionsupdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.0-preview2-35157")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TCGshopTestEnvironment.Models.Category", b =>
                {
                    b.Property<string>("CategoryName")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.HasKey("CategoryName");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("TCGshopTestEnvironment.Models.JoinTables.ProductCategory", b =>
                {
                    b.Property<int>("ProductId");

                    b.Property<string>("CategoryName");

                    b.HasKey("ProductId", "CategoryName");

                    b.HasIndex("CategoryName");

                    b.ToTable("ProductCategory");
                });

            modelBuilder.Entity("TCGshopTestEnvironment.Models.JoinTables.ShoppingBasket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<DateTime>("DateCreated");

                    b.Property<int>("ProductsId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Basket");
                });

            modelBuilder.Entity("TCGshopTestEnvironment.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<Guid>("Guid");

                    b.Property<string>("LastName");

                    b.Property<DateTime>("OrderDate");

                    b.Property<bool>("Paid");

                    b.Property<string>("PaymentId");

                    b.Property<string>("PaymentStatus");

                    b.Property<string>("Phone");

                    b.Property<string>("PostalCode");

                    b.Property<string>("State");

                    b.Property<decimal>("Total");

                    b.HasKey("OrderId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("TCGshopTestEnvironment.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("OrderId");

                    b.Property<int>("ProductId");

                    b.Property<int>("Quantity");

                    b.Property<decimal>("UnitPrice");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("TCGshopTestEnvironment.Models.Products", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AuctionEndTime");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Description");

                    b.Property<string>("Grade");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Name");

                    b.Property<string>("OwnerId");

                    b.Property<decimal>("Price");

                    b.Property<bool>("Removed");

                    b.Property<int>("Stock");

                    b.Property<int>("ViewsDetails");

                    b.Property<int>("ViewsListed");

                    b.HasKey("ProductId");

                    b.HasIndex("OwnerId");

                    b.ToTable("products");
                });

            modelBuilder.Entity("TCGshopTestEnvironment.Models.ProductsCat", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AuctionEndTime");

                    b.Property<string>("CategoryName");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Description");

                    b.Property<string>("Grade");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Name");

                    b.Property<string>("OwnerId");

                    b.Property<decimal>("Price");

                    b.Property<int>("Stock");

                    b.Property<int>("ViewsDetails");

                    b.Property<int>("ViewsListed");

                    b.HasKey("ProductId");

                    b.HasIndex("OwnerId");

                    b.ToTable("ProductsCat");
                });

            modelBuilder.Entity("TCGshopTestEnvironment.Models.Statistics", b =>
                {
                    b.Property<int>("Static_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.HasKey("Static_ID");

                    b.ToTable("statistics");
                });

            modelBuilder.Entity("TCGshopTestEnvironment.Models.UserAccount", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Address");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Country");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<string>("ZipCode");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TCGshopTestEnvironment.Models.Wishlist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProductId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("wishlists");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TCGshopTestEnvironment.Models.UserAccount")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TCGshopTestEnvironment.Models.UserAccount")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TCGshopTestEnvironment.Models.UserAccount")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TCGshopTestEnvironment.Models.UserAccount")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TCGshopTestEnvironment.Models.JoinTables.ProductCategory", b =>
                {
                    b.HasOne("TCGshopTestEnvironment.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryName")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TCGshopTestEnvironment.Models.Products", "Products")
                        .WithMany("Category")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TCGshopTestEnvironment.Models.OrderDetail", b =>
                {
                    b.HasOne("TCGshopTestEnvironment.Models.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TCGshopTestEnvironment.Models.Products", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TCGshopTestEnvironment.Models.Products", b =>
                {
                    b.HasOne("TCGshopTestEnvironment.Models.UserAccount", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("TCGshopTestEnvironment.Models.ProductsCat", b =>
                {
                    b.HasOne("TCGshopTestEnvironment.Models.UserAccount", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("TCGshopTestEnvironment.Models.Wishlist", b =>
                {
                    b.HasOne("TCGshopTestEnvironment.Models.Products", "Product")
                        .WithMany("User")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TCGshopTestEnvironment.Models.UserAccount", "User")
                        .WithMany("Products")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
