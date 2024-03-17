using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frieght.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixShipperInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loads_ShipperDto_CreatedById",
                table: "Loads");

            migrationBuilder.DropTable(
                name: "ShipperDto");

            migrationBuilder.DropIndex(
                name: "IX_Loads_CreatedById",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Loads");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Loads",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Shipper",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    DOTNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipper", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loads_CreatedByUserId",
                table: "Loads",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loads_Shipper_CreatedByUserId",
                table: "Loads",
                column: "CreatedByUserId",
                principalTable: "Shipper",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loads_Shipper_CreatedByUserId",
                table: "Loads");

            migrationBuilder.DropTable(
                name: "Shipper");

            migrationBuilder.DropIndex(
                name: "IX_Loads_CreatedByUserId",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Loads");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Loads",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShipperDto",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    DOTNumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipperDto", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loads_CreatedById",
                table: "Loads",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Loads_ShipperDto_CreatedById",
                table: "Loads",
                column: "CreatedById",
                principalTable: "ShipperDto",
                principalColumn: "Id");
        }
    }
}
