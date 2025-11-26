using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Danplanner.Migrations
{
    /// <inheritdoc />
    public partial class NewEntitiesAndUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalPurchases",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SeasonalPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "GrassFields");

            migrationBuilder.DropColumn(
                name: "PricePerNight",
                table: "GrassFields");

            migrationBuilder.DropColumn(
                name: "CancelBooking",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Rebook",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "ServicePrice",
                table: "Products",
                newName: "PricePerNight");

            migrationBuilder.RenameColumn(
                name: "NumberOfGuests",
                table: "Products",
                newName: "MaxGuests");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "GrassFields",
                newName: "Size");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Bookings",
                newName: "CampistId");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "GrassFields",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "GrassFields",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Cottages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CottageId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GrassFieldId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Bookings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Addons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addons", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Receipts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receipts_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BookingAddons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    AddonId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AddonsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingAddons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingAddons_Addons_AddonsId",
                        column: x => x.AddonsId,
                        principalTable: "Addons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingAddons_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GrassFields_ProductId",
                table: "GrassFields",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CottageId",
                table: "Bookings",
                column: "CottageId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_GrassFieldId",
                table: "Bookings",
                column: "GrassFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingAddons_AddonsId",
                table: "BookingAddons",
                column: "AddonsId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingAddons_BookingId",
                table: "BookingAddons",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_BookingId",
                table: "Receipts",
                column: "BookingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Cottages_CottageId",
                table: "Bookings",
                column: "CottageId",
                principalTable: "Cottages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_GrassFields_GrassFieldId",
                table: "Bookings",
                column: "GrassFieldId",
                principalTable: "GrassFields",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GrassFields_Products_ProductId",
                table: "GrassFields",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Cottages_CottageId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_GrassFields_GrassFieldId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_GrassFields_Products_ProductId",
                table: "GrassFields");

            migrationBuilder.DropTable(
                name: "BookingAddons");

            migrationBuilder.DropTable(
                name: "Receipts");

            migrationBuilder.DropTable(
                name: "Addons");

            migrationBuilder.DropIndex(
                name: "IX_GrassFields_ProductId",
                table: "GrassFields");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_CottageId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_GrassFieldId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "GrassFields");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "GrassFields");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Cottages");

            migrationBuilder.DropColumn(
                name: "CottageId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "GrassFieldId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "PricePerNight",
                table: "Products",
                newName: "ServicePrice");

            migrationBuilder.RenameColumn(
                name: "MaxGuests",
                table: "Products",
                newName: "NumberOfGuests");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "GrassFields",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CampistId",
                table: "Bookings",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalPurchases",
                table: "Products",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "SeasonalPrice",
                table: "Products",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GrassFields",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerNight",
                table: "GrassFields",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "CancelBooking",
                table: "Bookings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Rebook",
                table: "Bookings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
