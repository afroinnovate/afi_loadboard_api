using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Frieght.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class refactorUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loads_Shipper_ShipperUserId",
                table: "Loads");

            migrationBuilder.DropTable(
                name: "Carriers");

            migrationBuilder.DropTable(
                name: "Shipper");

            migrationBuilder.DropColumn(
                name: "AcceptedBidId",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Loads");

            migrationBuilder.RenameColumn(
                name: "Modified",
                table: "Loads",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Loads",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Loads",
                newName: "LoadId");

            migrationBuilder.AlterColumn<decimal>(
                name: "BidAmount",
                table: "Bids",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    MotorCarrierNumber = table.Column<string>(type: "text", nullable: true),
                    DOTNumber = table.Column<string>(type: "text", nullable: true),
                    EquipmentType = table.Column<string>(type: "text", nullable: true),
                    AvailableCapacity = table.Column<double>(type: "double precision", nullable: true),
                    CompanyName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loads_Origin_Destination",
                table: "Loads",
                columns: new[] { "Origin", "Destination" });

            migrationBuilder.CreateIndex(
                name: "IX_Bids_CarrierId",
                table: "Bids",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_LoadId",
                table: "Bids",
                column: "LoadId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Loads_LoadId",
                table: "Bids",
                column: "LoadId",
                principalTable: "Loads",
                principalColumn: "LoadId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Users_CarrierId",
                table: "Bids",
                column: "CarrierId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Loads_Users_ShipperUserId",
                table: "Loads",
                column: "ShipperUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Loads_LoadId",
                table: "Bids");

            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Users_CarrierId",
                table: "Bids");

            migrationBuilder.DropForeignKey(
                name: "FK_Loads_Users_ShipperUserId",
                table: "Loads");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Loads_Origin_Destination",
                table: "Loads");

            migrationBuilder.DropIndex(
                name: "IX_Bids_CarrierId",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_LoadId",
                table: "Bids");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Loads",
                newName: "Modified");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Loads",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "LoadId",
                table: "Loads",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "AcceptedBidId",
                table: "Loads",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Loads",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "BidAmount",
                table: "Bids",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.CreateTable(
                name: "Carriers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AvailableCapacity = table.Column<double>(type: "double precision", nullable: false),
                    CompanyEmail = table.Column<string>(type: "text", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    CompanyPhone = table.Column<string>(type: "text", nullable: false),
                    DOTNumber = table.Column<string>(type: "text", nullable: false),
                    EquipmentType = table.Column<string>(type: "text", nullable: false),
                    MotorCarrierNumber = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carriers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shipper",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    DOTNumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipper", x => x.UserId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Loads_Shipper_ShipperUserId",
                table: "Loads",
                column: "ShipperUserId",
                principalTable: "Shipper",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
