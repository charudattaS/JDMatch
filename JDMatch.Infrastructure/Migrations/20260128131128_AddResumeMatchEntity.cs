using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JDMatch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddResumeMatchEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "JobDescriptions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ResumeMatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResumeId = table.Column<Guid>(type: "uuid", nullable: false),
                    JobDescriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Score = table.Column<decimal>(type: "numeric", nullable: false),
                    Verdict = table.Column<string>(type: "text", nullable: false),
                    MatchedSkillsJson = table.Column<string>(type: "text", nullable: true),
                    MissingSkillsJson = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeMatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeMatches_JobDescriptions_JobDescriptionId",
                        column: x => x.JobDescriptionId,
                        principalTable: "JobDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResumeMatches_Resumes_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 28, 13, 11, 28, 511, DateTimeKind.Utc).AddTicks(600));

            migrationBuilder.UpdateData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 28, 13, 11, 28, 511, DateTimeKind.Utc).AddTicks(601));

            migrationBuilder.UpdateData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 28, 13, 11, 28, 511, DateTimeKind.Utc).AddTicks(603));

            migrationBuilder.CreateIndex(
                name: "IX_JobDescriptions_UserId",
                table: "JobDescriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeMatches_JobDescriptionId",
                table: "ResumeMatches",
                column: "JobDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeMatches_ResumeId",
                table: "ResumeMatches",
                column: "ResumeId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobDescriptions_Users_UserId",
                table: "JobDescriptions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobDescriptions_Users_UserId",
                table: "JobDescriptions");

            migrationBuilder.DropTable(
                name: "ResumeMatches");

            migrationBuilder.DropIndex(
                name: "IX_JobDescriptions_UserId",
                table: "JobDescriptions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "JobDescriptions");

            migrationBuilder.UpdateData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 28, 13, 1, 4, 426, DateTimeKind.Utc).AddTicks(582));

            migrationBuilder.UpdateData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 28, 13, 1, 4, 426, DateTimeKind.Utc).AddTicks(585));

            migrationBuilder.UpdateData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 28, 13, 1, 4, 426, DateTimeKind.Utc).AddTicks(586));
        }
    }
}
