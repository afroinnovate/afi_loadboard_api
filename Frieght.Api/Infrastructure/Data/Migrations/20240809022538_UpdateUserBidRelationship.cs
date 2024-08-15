using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frieght.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserBidRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Users_CarrierId",
                table: "Bids");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Users_CarrierId",
                table: "Bids",
                column: "CarrierId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Users_CarrierId",
                table: "Bids");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Users_CarrierId",
                table: "Bids",
                column: "CarrierId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
