using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_mainImage_for_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttribute_Category_CategoryId",
                table: "ProductAttribute");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDisplay",
                table: "ProductAttribute",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "ProductAttribute",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "CanFilter",
                table: "ProductAttribute",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MainImageId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_MainImageId",
                table: "Product",
                column: "MainImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ImageFile_MainImageId",
                table: "Product",
                column: "MainImageId",
                principalTable: "ImageFile",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttribute_Category_CategoryId",
                table: "ProductAttribute",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ImageFile_MainImageId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttribute_Category_CategoryId",
                table: "ProductAttribute");

            migrationBuilder.DropIndex(
                name: "IX_Product_MainImageId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "MainImageId",
                table: "Product");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDisplay",
                table: "ProductAttribute",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "ProductAttribute",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "CanFilter",
                table: "ProductAttribute",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttribute_Category_CategoryId",
                table: "ProductAttribute",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }
    }
}
