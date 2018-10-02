using Microsoft.EntityFrameworkCore.Migrations;

namespace TCGshopTestEnvironment.Migrations
{
    public partial class UpdateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Product_ID",
                table: "products",
                newName: "ProductID");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "products",
                newName: "Product_ID");
        }
    }
}
