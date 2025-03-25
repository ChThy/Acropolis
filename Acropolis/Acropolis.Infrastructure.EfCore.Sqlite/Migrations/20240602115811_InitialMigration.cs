using System;
using Acropolis.Application.Sagas.DownloadVideo;
using Acropolis.Application.Sagas.ExternalMessageRequest;
using Acropolis.Infrastructure.EfCore.Sqlite;
using Microsoft.EntityFrameworkCore.Migrations;

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
                name: "DownloadVideoState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<int>(type: "INTEGER", rowVersion: true, nullable: false, defaultValue: 0),
                    CurrentState = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, collation: "BINARY"),
                    Url = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY"),
                    RequestedTimestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DownloadedTimestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    VideoMetaData_VideoId = table.Column<string>(type: "TEXT", nullable: true, collation: "BINARY"),
                    VideoMetaData_VideoTitle = table.Column<string>(type: "TEXT", nullable: true, collation: "BINARY"),
                    VideoMetaData_Author = table.Column<string>(type: "TEXT", nullable: true, collation: "BINARY"),
                    VideoMetaData_VideoUploadTimestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    VideoMetaData_StorageLocation = table.Column<string>(type: "TEXT", nullable: true, collation: "BINARY"),
                    ErrorMessage = table.Column<string>(type: "TEXT", nullable: true, collation: "BINARY"),
                    ErrorTimestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadVideoState", x => x.CorrelationId);
                });

            migrationBuilder.CreateTable(
                name: "ExternalMessageRequestState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<int>(type: "INTEGER", rowVersion: true, nullable: false, defaultValue: 0),
                    CurrentState = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false, collation: "BINARY"),
                    Channel = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY"),
                    ReceivedOn = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    MessageBody = table.Column<string>(type: "TEXT", nullable: true, collation: "BINARY"),
                    MessageProps = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalMessageRequestState", x => x.CorrelationId);
                });

            migrationBuilder.CreateTable(
                name: "InboxStates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConsumerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LockId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true),
                    Received = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReceiveCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Consumed = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Delivered = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxStates", x => x.Id);
                    table.UniqueConstraint("AK_InboxStates_MessageId_ConsumerId", x => new { x.MessageId, x.ConsumerId });
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessage",
                columns: table => new
                {
                    SequenceNumber = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnqueueTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SentTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Headers = table.Column<string>(type: "TEXT", nullable: true, collation: "BINARY"),
                    Properties = table.Column<string>(type: "TEXT", nullable: true, collation: "BINARY"),
                    InboxMessageId = table.Column<Guid>(type: "TEXT", nullable: true),
                    InboxConsumerId = table.Column<Guid>(type: "TEXT", nullable: true),
                    OutboxId = table.Column<Guid>(type: "TEXT", nullable: true),
                    MessageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false, collation: "BINARY"),
                    MessageType = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY"),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY"),
                    ConversationId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CorrelationId = table.Column<Guid>(type: "TEXT", nullable: true),
                    InitiatorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    RequestId = table.Column<Guid>(type: "TEXT", nullable: true),
                    SourceAddress = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    DestinationAddress = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ResponseAddress = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    FaultAddress = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ExpirationTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.SequenceNumber);
                });

            migrationBuilder.CreateTable(
                name: "OutboxStates",
                columns: table => new
                {
                    OutboxId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LockId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Delivered = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxStates", x => x.OutboxId);
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
            
            if (migrationBuilder.IsSqlite())
            {
                migrationBuilder.Sql(Triggers.GetCreateRowVersionTriggerSql(nameof(DownloadVideoState)));
                migrationBuilder.Sql(Triggers.GetCreateRowVersionTriggerSql(nameof(ExternalMessageRequestState)));
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DownloadVideoState");

            migrationBuilder.DropTable(
                name: "ExternalMessageRequestState");

            migrationBuilder.DropTable(
                name: "InboxStates");

            migrationBuilder.DropTable(
                name: "OutboxMessage");

            migrationBuilder.DropTable(
                name: "OutboxStates");
        }
    }
}
