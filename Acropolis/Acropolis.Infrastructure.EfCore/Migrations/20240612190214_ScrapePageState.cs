using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acropolis.Infrastructure.EfCore.Messenger.Migrations
{
    /// <inheritdoc />
    public partial class ScrapePageState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScrapePageState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<int>(type: "INTEGER", rowVersion: true, nullable: false, defaultValue: 0),
                    CurrentState = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, collation: "BINARY"),
                    Url = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY"),
                    RequestedTimestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ScrapedTimestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Domain = table.Column<string>(type: "TEXT", nullable: true, collation: "BINARY"),
                    Title = table.Column<string>(type: "TEXT", nullable: true, collation: "BINARY"),
                    StorageLocation = table.Column<string>(type: "TEXT", nullable: true, collation: "BINARY"),
                    ErrorMessage = table.Column<string>(type: "TEXT", nullable: true, collation: "BINARY"),
                    ErrorTimestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapePageState", x => x.CorrelationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScrapePageState_CorrelationId_Url",
                table: "ScrapePageState",
                columns: new[] { "CorrelationId", "Url" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScrapePageState_Url",
                table: "ScrapePageState",
                column: "Url",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScrapePageState");
        }
    }
}
