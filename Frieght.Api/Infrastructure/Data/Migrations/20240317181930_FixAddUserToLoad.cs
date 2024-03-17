using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frieght.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixAddUserToLoad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loads_Shipper_CreatedById",
                table: "Loads");

            migrationBuilder.DropTable(
                name: "Shipper");

            migrationBuilder.CreateTable(
                name: "ShipperDto",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipperDto", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Loads_ShipperDto_CreatedById",
                table: "Loads",
                column: "CreatedById",
                principalTable: "ShipperDto",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loads_ShipperDto_CreatedById",
                table: "Loads");

            migrationBuilder.DropTable(
                name: "ShipperDto");

            migrationBuilder.CreateTable(
                name: "Shipper",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipper", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Loads_Shipper_CreatedById",
                table: "Loads",
                column: "CreatedById",
                principalTable: "Shipper",
                principalColumn: "Id");
        }
    }
}
