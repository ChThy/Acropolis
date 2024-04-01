using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acropolis.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddDownloadVideoStateIndices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DownloadVideoState_CorrelationId_Url",
                table: "DownloadVideoState",
                columns: new[] { "CorrelationId", "Url" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DownloadVideoState_Url",
                table: "DownloadVideoState",
                column: "Url",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DownloadVideoState_CorrelationId_Url",
                table: "DownloadVideoState");

            migrationBuilder.DropIndex(
                name: "IX_DownloadVideoState_Url",
                table: "DownloadVideoState");
        }
    }
}
