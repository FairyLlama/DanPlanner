using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Danplanner.Migrations
{
    /// <inheritdoc />
    public partial class GrassFieldFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxCapacity",
                table: "GrassFields",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxCapacity",
                table: "GrassFields");
        }
    }
}
