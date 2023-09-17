using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acropolis.Infrastructure.EfCore.Messenger.Migrations
{
    /// <inheritdoc />
    public partial class ExternalIdOnIncomingRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExternalId",
                table: "IncomingRequests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IncomingRequests_ExternalId",
                table: "IncomingRequests",
                column: "ExternalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IncomingRequests_ExternalId",
                table: "IncomingRequests");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "IncomingRequests");
        }
    }
}
