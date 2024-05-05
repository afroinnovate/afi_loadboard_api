using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Min.API.Migrations
{
    /// <inheritdoc />
    public partial class refactorAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DotNumber",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DotNumber",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
