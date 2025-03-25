using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acropolis.Infrastructure.EfCore.Messenger.Migrations
{
    /// <inheritdoc />
    public partial class DropMassTransitOutboxUpdatePackage823 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InboxStates");

            migrationBuilder.DropTable(
                name: "OutboxMessage");

            migrationBuilder.DropTable(
                name: "OutboxStates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InboxStates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Consumed = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ConsumerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Delivered = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExpirationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "INTEGER", nullable: true),
                    LockId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MessageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReceiveCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Received = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
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
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY"),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false, collation: "BINARY"),
                    ConversationId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CorrelationId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DestinationAddress = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EnqueueTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExpirationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FaultAddress = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Headers = table.Column<string>(type: "TEXT", nullable: true, collation: "BINARY"),
                    InboxConsumerId = table.Column<Guid>(type: "TEXT", nullable: true),
                    InboxMessageId = table.Column<Guid>(type: "TEXT", nullable: true),
                    InitiatorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    MessageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MessageType = table.Column<string>(type: "TEXT", nullable: false, collation: "BINARY"),
                    OutboxId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Properties = table.Column<string>(type: "TEXT", nullable: true, collation: "BINARY"),
                    RequestId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ResponseAddress = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    SentTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SourceAddress = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
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
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Delivered = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "INTEGER", nullable: true),
                    LockId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxStates", x => x.OutboxId);
                });

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
        }
    }
}
