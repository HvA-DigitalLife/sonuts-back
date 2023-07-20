using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sonuts.Infrastructure.Migrations
{
    public partial class AnswerOptionName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AnswerOptions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AnswerOptions");
        }
    }
}
