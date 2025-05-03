using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alter_column_color_productDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Color",
                table: "ProductDetail",
                newName: "ColorName");

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "ProductDetail",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "ProductDetail");

            migrationBuilder.RenameColumn(
                name: "ColorName",
                table: "ProductDetail",
                newName: "Color");
        }
    }
}
