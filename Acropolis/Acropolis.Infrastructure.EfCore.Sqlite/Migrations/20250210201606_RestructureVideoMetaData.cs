using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acropolis.Infrastructure.EfCore.Messenger.Migrations
{
    /// <inheritdoc />
    public partial class RestructureVideoMetaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VideoMetaData_VideoUploadTimestamp",
                table: "DownloadVideoState",
                newName: "StoredVideo_MetaData_VideoUploadTimestamp");

            migrationBuilder.RenameColumn(
                name: "VideoMetaData_VideoTitle",
                table: "DownloadVideoState",
                newName: "StoredVideo_MetaData_VideoTitle");

            migrationBuilder.RenameColumn(
                name: "VideoMetaData_VideoId",
                table: "DownloadVideoState",
                newName: "StoredVideo_MetaData_VideoId");

            migrationBuilder.RenameColumn(
                name: "VideoMetaData_StorageLocation",
                table: "DownloadVideoState",
                newName: "StoredVideo_StorageLocation");

            migrationBuilder.RenameColumn(
                name: "VideoMetaData_Author",
                table: "DownloadVideoState",
                newName: "StoredVideo_MetaData_Author");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StoredVideo_StorageLocation",
                table: "DownloadVideoState",
                newName: "VideoMetaData_StorageLocation");

            migrationBuilder.RenameColumn(
                name: "StoredVideo_MetaData_VideoUploadTimestamp",
                table: "DownloadVideoState",
                newName: "VideoMetaData_VideoUploadTimestamp");

            migrationBuilder.RenameColumn(
                name: "StoredVideo_MetaData_VideoTitle",
                table: "DownloadVideoState",
                newName: "VideoMetaData_VideoTitle");

            migrationBuilder.RenameColumn(
                name: "StoredVideo_MetaData_VideoId",
                table: "DownloadVideoState",
                newName: "VideoMetaData_VideoId");

            migrationBuilder.RenameColumn(
                name: "StoredVideo_MetaData_Author",
                table: "DownloadVideoState",
                newName: "VideoMetaData_Author");
        }
    }
}
