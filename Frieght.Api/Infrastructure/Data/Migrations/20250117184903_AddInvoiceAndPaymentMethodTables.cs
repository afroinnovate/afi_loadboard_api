using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Frieght.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceAndPaymentMethodTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CarrierVehicle");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CarrierVehicle");

            migrationBuilder.DropColumn(
                name: "TruckLength",
                table: "CarrierVehicle");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CarrierVehicle");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CarrierVehicle");

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LoadId = table.Column<int>(type: "integer", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ShipperId = table.Column<int>(type: "integer", nullable: false),
                    AmountDue = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalVat = table.Column<decimal>(type: "numeric", nullable: false),
                    Withholding = table.Column<decimal>(type: "numeric", nullable: false),
                    ServiceFees = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    TransactionId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PaymentType = table.Column<string>(type: "text", nullable: false),
                    BankName = table.Column<string>(type: "text", nullable: true),
                    BankAccount = table.Column<string>(type: "text", nullable: true),
                    AccountHolderName = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    CardMethod = table.Column<string>(type: "text", nullable: true),
                    PaymentMethodId = table.Column<string>(type: "text", nullable: true),
                    CardType = table.Column<string>(type: "text", nullable: true),
                    LastFourDigits = table.Column<string>(type: "text", nullable: true),
                    BillingAddress = table.Column<string>(type: "text", nullable: true),
                    InvoiceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentMethods_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_InvoiceId",
                table: "PaymentMethods",
                column: "InvoiceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "CarrierVehicle",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CarrierVehicle",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruckLength",
                table: "CarrierVehicle",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "CarrierVehicle",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CarrierVehicle",
                type: "text",
                nullable: true);
        }
    }
}
