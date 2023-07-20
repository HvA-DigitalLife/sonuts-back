using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sonuts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTinyHabits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TinyHabit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    TinyHabitText = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinyHabit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TinyHabit_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TinyHabit_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TinyHabit_CategoryId",
                table: "TinyHabit",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TinyHabit_ParticipantId",
                table: "TinyHabit",
                column: "ParticipantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TinyHabit");
        }
    }
}
