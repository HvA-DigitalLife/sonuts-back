using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sonuts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MotivationalMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MotivationalMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    MinPercentage = table.Column<int>(type: "integer", nullable: false),
                    MaxPercentage = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotivationalMessages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MotivationalMessages");
        }
    }
}
