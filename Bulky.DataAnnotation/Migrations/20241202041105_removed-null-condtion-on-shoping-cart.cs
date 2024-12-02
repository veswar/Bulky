using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removednullcondtiononshopingcart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopingCart_AspNetUsers_ApplicationUserId",
                table: "ShopingCart");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopingCart_Product_ProductId",
                table: "ShopingCart");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ShopingCart",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "ShopingCart",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopingCart_AspNetUsers_ApplicationUserId",
                table: "ShopingCart",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopingCart_Product_ProductId",
                table: "ShopingCart",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopingCart_AspNetUsers_ApplicationUserId",
                table: "ShopingCart");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopingCart_Product_ProductId",
                table: "ShopingCart");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ShopingCart",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "ShopingCart",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopingCart_AspNetUsers_ApplicationUserId",
                table: "ShopingCart",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopingCart_Product_ProductId",
                table: "ShopingCart",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");
        }
    }
}
