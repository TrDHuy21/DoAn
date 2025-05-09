using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_UrlName_for_BaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameFiler",
                table: "ProductAttribute",
                newName: "UrlName");

            migrationBuilder.AddColumn<string>(
                name: "UrlName",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UrlName",
                table: "ProductDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UrlName",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UrlName",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UrlName",
                table: "Brand",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UrlName",
                table: "ProductDetail");

            migrationBuilder.DropColumn(
                name: "UrlName",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "UrlName",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "UrlName",
                table: "Brand");

            migrationBuilder.RenameColumn(
                name: "UrlName",
                table: "ProductAttribute",
                newName: "NameFiler");
        }
    }
}
