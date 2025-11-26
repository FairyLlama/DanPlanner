using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Danplanner.Migrations
{
    /// <inheritdoc />
    public partial class NewEntitiesAndUpdatesAddon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingAddons_Addons_AddonsId",
                table: "BookingAddons");

            migrationBuilder.AlterColumn<int>(
                name: "AddonsId",
                table: "BookingAddons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingAddons_Addons_AddonsId",
                table: "BookingAddons",
                column: "AddonsId",
                principalTable: "Addons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingAddons_Addons_AddonsId",
                table: "BookingAddons");

            migrationBuilder.AlterColumn<int>(
                name: "AddonsId",
                table: "BookingAddons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingAddons_Addons_AddonsId",
                table: "BookingAddons",
                column: "AddonsId",
                principalTable: "Addons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
