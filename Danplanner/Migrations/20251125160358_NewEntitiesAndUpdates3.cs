using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Danplanner.Migrations
{
    /// <inheritdoc />
    public partial class NewEntitiesAndUpdates3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxGuests",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPeople",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfPeople",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "MaxGuests",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
