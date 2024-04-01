using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acropolis.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoMetaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoMetaData_Author",
                table: "DownloadVideoState",
                type: "TEXT",
                nullable: true,
                collation: "BINARY");

            migrationBuilder.AddColumn<string>(
                name: "VideoMetaData_StorageLocation",
                table: "DownloadVideoState",
                type: "TEXT",
                nullable: true,
                collation: "BINARY");

            migrationBuilder.AddColumn<string>(
                name: "VideoMetaData_VideoId",
                table: "DownloadVideoState",
                type: "TEXT",
                nullable: true,
                collation: "BINARY");

            migrationBuilder.AddColumn<string>(
                name: "VideoMetaData_VideoTitle",
                table: "DownloadVideoState",
                type: "TEXT",
                nullable: true,
                collation: "BINARY");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "VideoMetaData_VideoUploadTimestamp",
                table: "DownloadVideoState",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoMetaData_Author",
                table: "DownloadVideoState");

            migrationBuilder.DropColumn(
                name: "VideoMetaData_StorageLocation",
                table: "DownloadVideoState");

            migrationBuilder.DropColumn(
                name: "VideoMetaData_VideoId",
                table: "DownloadVideoState");

            migrationBuilder.DropColumn(
                name: "VideoMetaData_VideoTitle",
                table: "DownloadVideoState");

            migrationBuilder.DropColumn(
                name: "VideoMetaData_VideoUploadTimestamp",
                table: "DownloadVideoState");
        }
    }
}
