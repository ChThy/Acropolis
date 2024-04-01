using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acropolis.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoDownloadErrorProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "DownloadVideoState",
                type: "TEXT",
                nullable: true,
                collation: "BINARY");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ErrorTimestamp",
                table: "DownloadVideoState",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "DownloadVideoState");

            migrationBuilder.DropColumn(
                name: "ErrorTimestamp",
                table: "DownloadVideoState");
        }
    }
}
