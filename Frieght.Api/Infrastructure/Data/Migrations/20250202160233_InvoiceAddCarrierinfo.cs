using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frieght.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceAddCarrierinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CarrierBusinessName",
                table: "Invoices",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarrierEmail",
                table: "Invoices",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarrierName",
                table: "Invoices",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarrierPhone",
                table: "Invoices",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarrierBusinessName",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CarrierEmail",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CarrierName",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CarrierPhone",
                table: "Invoices");
        }
    }
}
