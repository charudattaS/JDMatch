using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JDMatch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Plans",
                columns: new[] { "Id", "CreatedAt", "MonthlyPrice", "MonthlyResumeLimit", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 27, 14, 14, 42, 599, DateTimeKind.Utc).AddTicks(8432), 999m, 50, "Starter" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 27, 14, 14, 42, 599, DateTimeKind.Utc).AddTicks(8434), 2999m, 300, "Pro" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 1, 27, 14, 14, 42, 599, DateTimeKind.Utc).AddTicks(8436), 4999m, 1000, "Agency" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));
        }
    }
}
