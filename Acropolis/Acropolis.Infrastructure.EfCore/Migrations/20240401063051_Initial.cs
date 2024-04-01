using System;
using Acropolis.Application.Sagas;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acropolis.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DownloadVideoState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<int>(type: "INTEGER", rowVersion: true, nullable: false, defaultValue: 0),
                    CurrentState = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, collation: "BINARY"),
                    RequestedTimestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadVideoState", x => x.CorrelationId);
                });

            if (migrationBuilder.IsSqlite())
            {
                migrationBuilder.Sql(Triggers.GetCreateRowVersionTriggerSql(nameof(DownloadVideoState)));
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DownloadVideoState");
        }
    }
}
