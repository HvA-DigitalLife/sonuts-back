using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sonuts.Infrastructure.Migrations
{
    public partial class ActivityQuetions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GoalQuestion",
                table: "Themes",
                newName: "GoalFrequencyQuestion");

            migrationBuilder.RenameColumn(
                name: "CurrentQuestion",
                table: "Themes",
                newName: "CurrentFrequencyQuestion");

            migrationBuilder.AddColumn<string>(
                name: "CurrentActivityQuestion",
                table: "Themes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoalActivityQuestion",
                table: "Themes",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentActivityQuestion",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "GoalActivityQuestion",
                table: "Themes");

            migrationBuilder.RenameColumn(
                name: "GoalFrequencyQuestion",
                table: "Themes",
                newName: "GoalQuestion");

            migrationBuilder.RenameColumn(
                name: "CurrentFrequencyQuestion",
                table: "Themes",
                newName: "CurrentQuestion");
        }
    }
}
