using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acropolis.Infrastructure.EfCore.Messenger.Migrations
{
    /// <inheritdoc />
    public partial class AddDenormalizedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DownloadedVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: false, collation: "BINARY"),
                    MetaData_VideoId = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY"),
                    MetaData_VideoTitle = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY"),
                    MetaData_Author = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY"),
                    MetaData_VideoUploadTimestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadedVideos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScrapedPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: false, collation: "BINARY"),
                    MetaData_PageTitle = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY"),
                    MetaData_Domain = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapedPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StorageLocation = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY"),
                    CreatedTimestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Views = table.Column<int>(type: "INTEGER", nullable: false),
                    DownloadedVideoId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ScrapedPageId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resource_DownloadedVideos_DownloadedVideoId",
                        column: x => x.DownloadedVideoId,
                        principalTable: "DownloadedVideos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Resource_ScrapedPages_ScrapedPageId",
                        column: x => x.ScrapedPageId,
                        principalTable: "ScrapedPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resource_DownloadedVideoId",
                table: "Resource",
                column: "DownloadedVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_ScrapedPageId",
                table: "Resource",
                column: "ScrapedPageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "DownloadedVideos");

            migrationBuilder.DropTable(
                name: "ScrapedPages");
        }
    }
}
