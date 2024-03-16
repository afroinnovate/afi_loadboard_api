using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frieght.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserToLoad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Loads",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Bids",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Shipper",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipper", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loads_CreatedById",
                table: "Loads",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Loads_Shipper_CreatedById",
                table: "Loads",
                column: "CreatedById",
                principalTable: "Shipper",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loads_Shipper_CreatedById",
                table: "Loads");

            migrationBuilder.DropTable(
                name: "Shipper");

            migrationBuilder.DropIndex(
                name: "IX_Loads_CreatedById",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Loads");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Bids",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
