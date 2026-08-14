using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaturaAi.Migrations
{
    /// <inheritdoc />
    public partial class AddExamAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "UserAnswers");

            migrationBuilder.AddColumn<int>(
                name: "AnswerId",
                table: "UserAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnswerText",
                table: "UserAnswers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndAt",
                table: "ExamAttempts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ExamId",
                table: "ExamAttempts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "ExamAttempts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "ExamAttempts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ExamAttempts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_AnswerId",
                table: "UserAnswers",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_ExamAttemptId",
                table: "UserAnswers",
                column: "ExamAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_QuestionId",
                table: "UserAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAttempts_ExamId",
                table: "ExamAttempts",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAttempts_UserId",
                table: "ExamAttempts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAttempts_AspNetUsers_UserId",
                table: "ExamAttempts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAttempts_Exam_ExamId",
                table: "ExamAttempts",
                column: "ExamId",
                principalSchema: "dbo",
                principalTable: "Exam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Answers_AnswerId",
                table: "UserAnswers",
                column: "AnswerId",
                principalSchema: "dbo",
                principalTable: "Answers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_ExamAttempts_ExamAttemptId",
                table: "UserAnswers",
                column: "ExamAttemptId",
                principalTable: "ExamAttempts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Questions_QuestionId",
                table: "UserAnswers",
                column: "QuestionId",
                principalSchema: "dbo",
                principalTable: "Questions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamAttempts_AspNetUsers_UserId",
                table: "ExamAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamAttempts_Exam_ExamId",
                table: "ExamAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Answers_AnswerId",
                table: "UserAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_ExamAttempts_ExamAttemptId",
                table: "UserAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Questions_QuestionId",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_AnswerId",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_ExamAttemptId",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_QuestionId",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_ExamAttempts_ExamId",
                table: "ExamAttempts");

            migrationBuilder.DropIndex(
                name: "IX_ExamAttempts_UserId",
                table: "ExamAttempts");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "AnswerText",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "EndAt",
                table: "ExamAttempts");

            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "ExamAttempts");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "ExamAttempts");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "ExamAttempts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ExamAttempts");

            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "UserAnswers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
