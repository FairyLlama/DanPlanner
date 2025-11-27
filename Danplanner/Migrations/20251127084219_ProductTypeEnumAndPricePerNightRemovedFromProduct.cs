using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Danplanner.Migrations
{
    /// <inheritdoc />
    public partial class ProductTypeEnumAndPricePerNightRemovedFromProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerNight",
                table: "Products");

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerNight",
                table: "GrassFields",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerNight",
                table: "Cottages",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerNight",
                table: "GrassFields");

            migrationBuilder.DropColumn(
                name: "PricePerNight",
                table: "Cottages");

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerNight",
                table: "Products",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
