using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sonuts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Information : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "Questions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Information",
                table: "Questions");
        }
    }
}
