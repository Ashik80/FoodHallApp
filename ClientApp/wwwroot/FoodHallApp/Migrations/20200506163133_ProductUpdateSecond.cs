using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodHallApp.Migrations
{
    public partial class ProductUpdateSecond : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCategory",
                table: "Product");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductCategory",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
