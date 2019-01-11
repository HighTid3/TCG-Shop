using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TCGshopTestEnvironment.Migrations
{
    public partial class auctionsupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AutionEndTime",
                table: "products",
                newName: "AuctionEndTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "AuctionEndTime",
                table: "ProductsCat",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuctionEndTime",
                table: "ProductsCat");

            migrationBuilder.RenameColumn(
                name: "AuctionEndTime",
                table: "products",
                newName: "AutionEndTime");
        }
    }
}
