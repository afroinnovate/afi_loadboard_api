using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frieght.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrierVehicle_BusinessProfiles_Id",
                table: "CarrierVehicle");

            migrationBuilder.DropColumn(
                name: "BusinessProfileId",
                table: "CarrierVehicle");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CarrierVehicle",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarrierVehicle_BusinessProfiles_Id",
                table: "CarrierVehicle",
                column: "Id",
                principalTable: "BusinessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrierVehicle_BusinessProfiles_Id",
                table: "CarrierVehicle");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CarrierVehicle",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "BusinessProfileId",
                table: "CarrierVehicle",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarrierVehicle_BusinessProfiles_Id",
                table: "CarrierVehicle",
                column: "Id",
                principalTable: "BusinessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
