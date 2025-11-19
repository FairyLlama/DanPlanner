using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp3semesterEksamensProjektMappe.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateSqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Resources_ResourceId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ResourceId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "ProductType");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Products",
                newName: "AdditionalPurchases");

            migrationBuilder.RenameColumn(
                name: "BasePrice",
                table: "Products",
                newName: "ServicePrice");

            migrationBuilder.RenameColumn(
                name: "ResourceId",
                table: "Bookings",
                newName: "UserId");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfGuests",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SeasonalPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "CancelBooking",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Rebook",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Huts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaxCapacity = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Huts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Huts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Huts_ProductId",
                table: "Huts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Huts");

            migrationBuilder.DropColumn(
                name: "NumberOfGuests",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SeasonalPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CancelBooking",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Rebook",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "ServicePrice",
                table: "Products",
                newName: "BasePrice");

            migrationBuilder.RenameColumn(
                name: "ProductType",
                table: "Products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "AdditionalPurchases",
                table: "Products",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Bookings",
                newName: "ResourceId");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ResourceId",
                table: "Bookings",
                column: "ResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Resources_ResourceId",
                table: "Bookings",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
