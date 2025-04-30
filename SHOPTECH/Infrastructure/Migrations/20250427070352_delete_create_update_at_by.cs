using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class delete_create_update_at_by : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brand_User_CreatedBy",
                table: "Brand");

            migrationBuilder.DropForeignKey(
                name: "FK_Brand_User_UpdatedBy",
                table: "Brand");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_User_CreatedBy",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_User_UpdatedBy",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_User_CreatedBy",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_User_UpdatedBy",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttribute_User_CreatedBy",
                table: "ProductAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttribute_User_UpdatedBy",
                table: "ProductAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetail_User_CreatedBy",
                table: "ProductDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetail_User_UpdatedBy",
                table: "ProductDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_CreatedBy",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_UpdatedBy",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_CreatedBy",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_UpdatedBy",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_ProductDetail_CreatedBy",
                table: "ProductDetail");

            migrationBuilder.DropIndex(
                name: "IX_ProductDetail_UpdatedBy",
                table: "ProductDetail");

            migrationBuilder.DropIndex(
                name: "IX_ProductAttribute_CreatedBy",
                table: "ProductAttribute");

            migrationBuilder.DropIndex(
                name: "IX_ProductAttribute_UpdatedBy",
                table: "ProductAttribute");

            migrationBuilder.DropIndex(
                name: "IX_Product_CreatedBy",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_UpdatedBy",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Category_CreatedBy",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_UpdatedBy",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Brand_CreatedBy",
                table: "Brand");

            migrationBuilder.DropIndex(
                name: "IX_Brand_UpdatedBy",
                table: "Brand");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductDetail");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProductDetail");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProductDetail");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ProductDetail");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductAttribute");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProductAttribute");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProductAttribute");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ProductAttribute");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Brand");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Brand");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Brand");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Brand");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductDetail",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "ProductDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductDetail",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "ProductDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductAttribute",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "ProductAttribute",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductAttribute",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "ProductAttribute",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Product",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Product",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Category",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Category",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Brand",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Brand",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Brand",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Brand",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedBy",
                table: "User",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_User_UpdatedBy",
                table: "User",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_CreatedBy",
                table: "ProductDetail",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_UpdatedBy",
                table: "ProductDetail",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttribute_CreatedBy",
                table: "ProductAttribute",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttribute_UpdatedBy",
                table: "ProductAttribute",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CreatedBy",
                table: "Product",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Product_UpdatedBy",
                table: "Product",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Category_CreatedBy",
                table: "Category",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Category_UpdatedBy",
                table: "Category",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Brand_CreatedBy",
                table: "Brand",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Brand_UpdatedBy",
                table: "Brand",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Brand_User_CreatedBy",
                table: "Brand",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Brand_User_UpdatedBy",
                table: "Brand",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_User_CreatedBy",
                table: "Category",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_User_UpdatedBy",
                table: "Category",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_User_CreatedBy",
                table: "Product",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_User_UpdatedBy",
                table: "Product",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttribute_User_CreatedBy",
                table: "ProductAttribute",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttribute_User_UpdatedBy",
                table: "ProductAttribute",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetail_User_CreatedBy",
                table: "ProductDetail",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetail_User_UpdatedBy",
                table: "ProductDetail",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_CreatedBy",
                table: "User",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_UpdatedBy",
                table: "User",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
