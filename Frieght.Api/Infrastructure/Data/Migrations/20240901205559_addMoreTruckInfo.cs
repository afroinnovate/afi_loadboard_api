using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frieght.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class addMoreTruckInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loads_Users_ShipperUserId",
                table: "Loads");

            migrationBuilder.AddForeignKey(
                name: "FK_Loads_Users_ShipperUserId",
                table: "Loads",
                column: "ShipperUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loads_Users_ShipperUserId",
                table: "Loads");

            migrationBuilder.AddForeignKey(
                name: "FK_Loads_Users_ShipperUserId",
                table: "Loads",
                column: "ShipperUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
