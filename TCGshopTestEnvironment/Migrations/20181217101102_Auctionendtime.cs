using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TCGshopTestEnvironment.Migrations
{
    public partial class Auctionendtime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AutionEndTime",
                table: "products",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutionEndTime",
                table: "products");
        }
    }
}
