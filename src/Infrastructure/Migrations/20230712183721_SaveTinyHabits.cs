using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sonuts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SaveTinyHabits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TinyHabit_Categories_CategoryId",
                table: "TinyHabit");

            migrationBuilder.DropForeignKey(
                name: "FK_TinyHabit_Participants_ParticipantId",
                table: "TinyHabit");

            migrationBuilder.DropIndex(
                name: "IX_TinyHabit_CategoryId",
                table: "TinyHabit");

            migrationBuilder.DropIndex(
                name: "IX_TinyHabit_ParticipantId",
                table: "TinyHabit");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "TinyHabit");

            migrationBuilder.AlterColumn<string>(
                name: "TinyHabitText",
                table: "TinyHabit",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParticipantId",
                table: "TinyHabit",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedAt",
                table: "TinyHabit",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TinyHabitText",
                table: "TinyHabit",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ParticipantId",
                table: "TinyHabit",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedAt",
                table: "TinyHabit",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "TinyHabit",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TinyHabit_CategoryId",
                table: "TinyHabit",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TinyHabit_ParticipantId",
                table: "TinyHabit",
                column: "ParticipantId");

            migrationBuilder.AddForeignKey(
                name: "FK_TinyHabit_Categories_CategoryId",
                table: "TinyHabit",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TinyHabit_Participants_ParticipantId",
                table: "TinyHabit",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
