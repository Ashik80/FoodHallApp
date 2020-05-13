using Microsoft.EntityFrameworkCore.Migrations;

namespace ClientApp.Migrations
{
    public partial class ForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrderedProducts_ProductID",
                table: "OrderedProducts",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedProducts_Product_ProductID",
                table: "OrderedProducts",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedProducts_Product_ProductID",
                table: "OrderedProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderedProducts_ProductID",
                table: "OrderedProducts");
        }
    }
}
