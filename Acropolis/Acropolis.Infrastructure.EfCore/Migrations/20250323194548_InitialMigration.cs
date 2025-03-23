using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Acropolis.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DownloadedVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    MetaData_VideoId = table.Column<string>(type: "text", nullable: false),
                    MetaData_VideoTitle = table.Column<string>(type: "text", nullable: false),
                    MetaData_Author = table.Column<string>(type: "text", nullable: false),
                    MetaData_VideoUploadTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadedVideos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DownloadVideoState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowVersion = table.Column<int>(type: "integer", rowVersion: true, nullable: false, defaultValue: 0),
                    CurrentState = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    RequestedTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DownloadedTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StoredVideo_MetaData_VideoId = table.Column<string>(type: "text", nullable: true),
                    StoredVideo_MetaData_VideoTitle = table.Column<string>(type: "text", nullable: true),
                    StoredVideo_MetaData_Author = table.Column<string>(type: "text", nullable: true),
                    StoredVideo_MetaData_VideoUploadTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StoredVideo_StorageLocation = table.Column<string>(type: "text", nullable: true),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    ErrorTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadVideoState", x => x.CorrelationId);
                });

            migrationBuilder.CreateTable(
                name: "ExternalMessageRequestState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowVersion = table.Column<int>(type: "integer", rowVersion: true, nullable: false, defaultValue: 0),
                    CurrentState = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Channel = table.Column<string>(type: "text", nullable: false),
                    ReceivedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MessageBody = table.Column<string>(type: "text", nullable: true),
                    MessageProps = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalMessageRequestState", x => x.CorrelationId);
                });

            migrationBuilder.CreateTable(
                name: "InboxStates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsumerId = table.Column<Guid>(type: "uuid", nullable: false),
                    LockId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    Received = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReceiveCount = table.Column<int>(type: "integer", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Consumed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxStates", x => x.Id);
                    table.UniqueConstraint("AK_InboxStates_MessageId_ConsumerId", x => new { x.MessageId, x.ConsumerId });
                });

            migrationBuilder.CreateTable(
                name: "OutboxStates",
                columns: table => new
                {
                    OutboxId = table.Column<Guid>(type: "uuid", nullable: false),
                    LockId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxStates", x => x.OutboxId);
                });

            migrationBuilder.CreateTable(
                name: "ScrapedPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    MetaData_PageTitle = table.Column<string>(type: "text", nullable: false),
                    MetaData_Domain = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapedPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScrapePageState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowVersion = table.Column<int>(type: "integer", rowVersion: true, nullable: false, defaultValue: 0),
                    CurrentState = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    RequestedTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ScrapedTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Domain = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    StorageLocation = table.Column<string>(type: "text", nullable: true),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    ErrorTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapePageState", x => x.CorrelationId);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessage",
                columns: table => new
                {
                    SequenceNumber = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EnqueueTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SentTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Headers = table.Column<string>(type: "text", nullable: true),
                    Properties = table.Column<string>(type: "text", nullable: true),
                    InboxMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    InboxConsumerId = table.Column<Guid>(type: "uuid", nullable: true),
                    OutboxId = table.Column<Guid>(type: "uuid", nullable: true),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentType = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    MessageType = table.Column<string>(type: "text", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: true),
                    InitiatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: true),
                    SourceAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DestinationAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ResponseAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    FaultAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.SequenceNumber);
                    table.ForeignKey(
                        name: "FK_OutboxMessage_InboxStates_InboxMessageId_InboxConsumerId",
                        columns: x => new { x.InboxMessageId, x.InboxConsumerId },
                        principalTable: "InboxStates",
                        principalColumns: new[] { "MessageId", "ConsumerId" });
                    table.ForeignKey(
                        name: "FK_OutboxMessage_OutboxStates_OutboxId",
                        column: x => x.OutboxId,
                        principalTable: "OutboxStates",
                        principalColumn: "OutboxId");
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StorageLocation = table.Column<string>(type: "text", nullable: false),
                    CreatedTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Views = table.Column<int>(type: "integer", nullable: false),
                    DownloadedVideoId = table.Column<Guid>(type: "uuid", nullable: true),
                    ScrapedPageId = table.Column<Guid>(type: "uuid", nullable: true)
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
                name: "IX_DownloadVideoState_CorrelationId_Url",
                table: "DownloadVideoState",
                columns: new[] { "CorrelationId", "Url" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DownloadVideoState_Url",
                table: "DownloadVideoState",
                column: "Url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InboxStates_Delivered",
                table: "InboxStates",
                column: "Delivered");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_EnqueueTime",
                table: "OutboxMessage",
                column: "EnqueueTime");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_ExpirationTime",
                table: "OutboxMessage",
                column: "ExpirationTime");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_InboxMessageId_InboxConsumerId_SequenceNumber",
                table: "OutboxMessage",
                columns: new[] { "InboxMessageId", "InboxConsumerId", "SequenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_OutboxId_SequenceNumber",
                table: "OutboxMessage",
                columns: new[] { "OutboxId", "SequenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboxStates_Created",
                table: "OutboxStates",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_DownloadedVideoId",
                table: "Resource",
                column: "DownloadedVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_ScrapedPageId",
                table: "Resource",
                column: "ScrapedPageId");

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
                name: "DownloadVideoState");

            migrationBuilder.DropTable(
                name: "ExternalMessageRequestState");

            migrationBuilder.DropTable(
                name: "OutboxMessage");

            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "ScrapePageState");

            migrationBuilder.DropTable(
                name: "InboxStates");

            migrationBuilder.DropTable(
                name: "OutboxStates");

            migrationBuilder.DropTable(
                name: "DownloadedVideos");

            migrationBuilder.DropTable(
                name: "ScrapedPages");
        }
    }
}
