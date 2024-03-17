using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frieght.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCreatedByInLoad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "ShipperDto",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DOTNumber",
                table: "ShipperDto",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "ShipperDto");

            migrationBuilder.DropColumn(
                name: "DOTNumber",
                table: "ShipperDto");
        }
    }
}
