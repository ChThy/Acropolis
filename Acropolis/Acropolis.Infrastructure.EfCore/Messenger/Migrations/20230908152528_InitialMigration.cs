using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acropolis.Infrastructure.EfCore.Messenger.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "IncomingRequests",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                User_ExternalId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false, collation: "BINARY"),
                User_Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false, collation: "BINARY"),
                Source = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY"),
                Timestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                ProcessedOn = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                RawContent = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IncomingRequests", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_IncomingRequests_ProcessedOn",
            table: "IncomingRequests",
            column: "ProcessedOn");

        migrationBuilder.CreateIndex(
            name: "IX_IncomingRequests_Source",
            table: "IncomingRequests",
            column: "Source");

        migrationBuilder.CreateIndex(
            name: "IX_IncomingRequests_Timestamp",
            table: "IncomingRequests",
            column: "Timestamp");

        migrationBuilder.CreateIndex(
            name: "IX_IncomingRequests_User_ExternalId",
            table: "IncomingRequests",
            column: "User_ExternalId");

        migrationBuilder.CreateIndex(
            name: "IX_IncomingRequests_User_Name",
            table: "IncomingRequests",
            column: "User_Name");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "IncomingRequests");
    }
}
