using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frieght.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class PreventVehicleTypeDuplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VIN",
                table: "VehicleTypes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_VehicleTypes_VIN",
                table: "VehicleTypes",
                column: "VIN");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypes_VIN",
                table: "VehicleTypes",
                column: "VIN",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_VehicleTypes_VIN",
                table: "VehicleTypes");

            migrationBuilder.DropIndex(
                name: "IX_VehicleTypes_VIN",
                table: "VehicleTypes");

            migrationBuilder.AlterColumn<string>(
                name: "VIN",
                table: "VehicleTypes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
