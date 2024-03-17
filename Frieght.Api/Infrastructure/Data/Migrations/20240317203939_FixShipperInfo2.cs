using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frieght.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixShipperInfo2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loads_Shipper_CreatedByUserId",
                table: "Loads");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Loads",
                newName: "ShipperUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Loads_CreatedByUserId",
                table: "Loads",
                newName: "IX_Loads_ShipperUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loads_Shipper_ShipperUserId",
                table: "Loads",
                column: "ShipperUserId",
                principalTable: "Shipper",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loads_Shipper_ShipperUserId",
                table: "Loads");

            migrationBuilder.RenameColumn(
                name: "ShipperUserId",
                table: "Loads",
                newName: "CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Loads_ShipperUserId",
                table: "Loads",
                newName: "IX_Loads_CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loads_Shipper_CreatedByUserId",
                table: "Loads",
                column: "CreatedByUserId",
                principalTable: "Shipper",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
