using System;
using Acropolis.Application.Sagas.ExternalMessageRequest;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acropolis.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalMessageRequestState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalMessageRequestState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<int>(type: "INTEGER", rowVersion: true, nullable: false, defaultValue: 0),
                    CurrentState = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, collation: "BINARY"),
                    Channel = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY"),
                    OriginatingExternalMessageId = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY"),
                    ReceivedOn = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    MessageBody = table.Column<string>(type: "TEXT", nullable: true, collation: "BINARY"),
                    MessageProps = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalMessageRequestState", x => x.CorrelationId);
                });
            
            if (migrationBuilder.IsSqlite())
            {
                migrationBuilder.Sql(Triggers.GetCreateRowVersionTriggerSql(nameof(ExternalMessageRequestState)));
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalMessageRequestState");
        }
    }
}
