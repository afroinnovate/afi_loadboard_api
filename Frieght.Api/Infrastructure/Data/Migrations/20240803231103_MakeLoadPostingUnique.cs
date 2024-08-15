using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frieght.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeLoadPostingUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Loads_ShipperUserId",
                table: "Loads");

            migrationBuilder.CreateIndex(
                name: "IX_Loads_ShipperUserId_Origin_Destination_PickupDate_DeliveryD~",
                table: "Loads",
                columns: new[] { "ShipperUserId", "Origin", "Destination", "PickupDate", "DeliveryDate", "Commodity", "LoadDetails" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Loads_ShipperUserId_Origin_Destination_PickupDate_DeliveryD~",
                table: "Loads");

            migrationBuilder.CreateIndex(
                name: "IX_Loads_ShipperUserId",
                table: "Loads",
                column: "ShipperUserId");
        }
    }
}
