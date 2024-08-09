using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Frieght.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserRelationsUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrierVehicle_BusinessProfiles_Id",
                table: "CarrierVehicle");

            migrationBuilder.DropIndex(
                name: "IX_CarrierVehicle_Id_LicensePlate_VIN",
                table: "CarrierVehicle");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CarrierVehicle",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "BusinessProfileId",
                table: "CarrierVehicle",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CarrierVehicleId",
                table: "BusinessProfiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CarrierVehicle_BusinessProfileId",
                table: "CarrierVehicle",
                column: "BusinessProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CarrierVehicle_LicensePlate_VIN",
                table: "CarrierVehicle",
                columns: new[] { "LicensePlate", "VIN" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarrierVehicle_BusinessProfiles_BusinessProfileId",
                table: "CarrierVehicle",
                column: "BusinessProfileId",
                principalTable: "BusinessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrierVehicle_BusinessProfiles_BusinessProfileId",
                table: "CarrierVehicle");

            migrationBuilder.DropIndex(
                name: "IX_CarrierVehicle_BusinessProfileId",
                table: "CarrierVehicle");

            migrationBuilder.DropIndex(
                name: "IX_CarrierVehicle_LicensePlate_VIN",
                table: "CarrierVehicle");

            migrationBuilder.DropColumn(
                name: "BusinessProfileId",
                table: "CarrierVehicle");

            migrationBuilder.DropColumn(
                name: "CarrierVehicleId",
                table: "BusinessProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CarrierVehicle",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_CarrierVehicle_Id_LicensePlate_VIN",
                table: "CarrierVehicle",
                columns: new[] { "Id", "LicensePlate", "VIN" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarrierVehicle_BusinessProfiles_Id",
                table: "CarrierVehicle",
                column: "Id",
                principalTable: "BusinessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
