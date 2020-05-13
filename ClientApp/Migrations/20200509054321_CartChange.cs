using Microsoft.EntityFrameworkCore.Migrations;

namespace ClientApp.Migrations
{
    public partial class CartChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Cart");

            migrationBuilder.AddColumn<int>(
                name: "CustomerID",
                table: "Cart",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "Cart",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cart_ProductID",
                table: "Cart",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Product_ProductID",
                table: "Cart",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Product_ProductID",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_ProductID",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "Cart");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Cart",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
