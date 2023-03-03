using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sonuts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UnitAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitDescription",
                table: "Themes");

            migrationBuilder.AddColumn<int>(
                name: "UnitAmount",
                table: "Themes",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitAmount",
                table: "Themes");

            migrationBuilder.AddColumn<string>(
                name: "UnitDescription",
                table: "Themes",
                type: "text",
                nullable: true);
        }
    }
}
