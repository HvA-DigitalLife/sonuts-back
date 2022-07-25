using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sonuts.Infrastructure.Migrations
{
    public partial class Recommendations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecommendationRule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Operator = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ThemeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendationRule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecommendationRule_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestionRecommendationRule",
                columns: table => new
                {
                    QuestionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecommendationRulesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionRecommendationRule", x => new { x.QuestionsId, x.RecommendationRulesId });
                    table.ForeignKey(
                        name: "FK_QuestionRecommendationRule_Questions_QuestionsId",
                        column: x => x.QuestionsId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionRecommendationRule_RecommendationRule_Recommendatio~",
                        column: x => x.RecommendationRulesId,
                        principalTable: "RecommendationRule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionRecommendationRule_RecommendationRulesId",
                table: "QuestionRecommendationRule",
                column: "RecommendationRulesId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationRule_ThemeId",
                table: "RecommendationRule",
                column: "ThemeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionRecommendationRule");

            migrationBuilder.DropTable(
                name: "RecommendationRule");
        }
    }
}
