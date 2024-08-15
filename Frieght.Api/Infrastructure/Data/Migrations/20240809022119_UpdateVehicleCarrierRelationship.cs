using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frieght.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVehicleCarrierRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessVehicleTypes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_VehicleTypes_VIN",
                table: "VehicleTypes");

            migrationBuilder.DropIndex(
                name: "IX_VehicleTypes_VIN",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "HasInspection",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "HasInsurance",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "HasRegistration",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "LicensePlate",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "Make",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "VIN",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "VehicleTypes");

            migrationBuilder.CreateTable(
                name: "CarrierVehicle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    VehicleTypeId = table.Column<int>(type: "integer", nullable: false),
                    BusinessProfileId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    VIN = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LicensePlate = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Make = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "text", nullable: true),
                    Year = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HasInsurance = table.Column<bool>(type: "boolean", nullable: true),
                    HasRegistration = table.Column<bool>(type: "boolean", nullable: true),
                    HasInspection = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarrierVehicle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarrierVehicle_BusinessProfiles_Id",
                        column: x => x.Id,
                        principalTable: "BusinessProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarrierVehicle_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypes_Id_Name",
                table: "VehicleTypes",
                columns: new[] { "Id", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarrierVehicle_Id_LicensePlate_VIN",
                table: "CarrierVehicle",
                columns: new[] { "Id", "LicensePlate", "VIN" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarrierVehicle_VehicleTypeId",
                table: "CarrierVehicle",
                column: "VehicleTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarrierVehicle");

            migrationBuilder.DropIndex(
                name: "IX_VehicleTypes_Id_Name",
                table: "VehicleTypes");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "VehicleTypes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "VehicleTypes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasInspection",
                table: "VehicleTypes",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasInsurance",
                table: "VehicleTypes",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasRegistration",
                table: "VehicleTypes",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "VehicleTypes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicensePlate",
                table: "VehicleTypes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Make",
                table: "VehicleTypes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "VehicleTypes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VIN",
                table: "VehicleTypes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "VehicleTypes",
                type: "text",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_VehicleTypes_VIN",
                table: "VehicleTypes",
                column: "VIN");

            migrationBuilder.CreateTable(
                name: "BusinessVehicleTypes",
                columns: table => new
                {
                    BusinessProfileId = table.Column<int>(type: "integer", nullable: false),
                    VehicleTypeId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessVehicleTypes", x => new { x.BusinessProfileId, x.VehicleTypeId });
                    table.ForeignKey(
                        name: "FK_BusinessVehicleTypes_BusinessProfiles_BusinessProfileId",
                        column: x => x.BusinessProfileId,
                        principalTable: "BusinessProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessVehicleTypes_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypes_VIN",
                table: "VehicleTypes",
                column: "VIN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessVehicleTypes_VehicleTypeId",
                table: "BusinessVehicleTypes",
                column: "VehicleTypeId");
        }
    }
}
