using Microsoft.EntityFrameworkCore.Migrations;

namespace TCGshopTestEnvironment.Migrations
{
    public partial class productremovedcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Removed",
                table: "products",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Removed",
                table: "products");
        }
    }
}
