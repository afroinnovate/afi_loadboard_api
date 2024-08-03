using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frieght.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefineTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "Bids");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "Bids",
                type: "text",
                nullable: true);
        }
    }
}
