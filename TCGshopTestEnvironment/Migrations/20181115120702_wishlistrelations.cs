using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TCGshopTestEnvironment.Migrations
{
    public partial class wishlistrelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "whishlists");

            migrationBuilder.CreateTable(
                name: "ProductsCat",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    OwnerId = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Grade = table.Column<string>(nullable: true),
                    Stock = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    ViewsListed = table.Column<int>(nullable: false),
                    ViewsDetails = table.Column<int>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    CategoryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsCat", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_ProductsCat_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "wishlists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<string>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wishlists", x => new { x.Id });
                    table.UniqueConstraint("AK_wishlists_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_wishlists_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_wishlists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsCat_OwnerId",
                table: "ProductsCat",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_wishlists_UserId",
                table: "wishlists",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsCat");

            migrationBuilder.DropTable(
                name: "wishlists");

            migrationBuilder.CreateTable(
                name: "whishlists",
                columns: table => new
                {
                    Whishlist_ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_whishlists", x => x.Whishlist_ID);
                });
        }
    }
}
