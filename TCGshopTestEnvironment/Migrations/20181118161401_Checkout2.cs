using Microsoft.EntityFrameworkCore.Migrations;

namespace TCGshopTestEnvironment.Migrations
{
    public partial class Checkout2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "paymentId",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "paymentId",
                table: "Orders");
        }
    }
}
