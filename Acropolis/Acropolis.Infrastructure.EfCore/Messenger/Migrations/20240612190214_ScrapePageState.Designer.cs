﻿// <auto-generated />
using System;
using Acropolis.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Acropolis.Infrastructure.EfCore.Messenger.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240612190214_ScrapePageState")]
    partial class ScrapePageState
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("BINARY")
                .HasAnnotation("ProductVersion", "8.0.3");

            modelBuilder.Entity("Acropolis.Application.Sagas.DownloadVideo.DownloadVideoState", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrentState")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<DateTimeOffset?>("DownloadedTimestamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<DateTimeOffset?>("ErrorTimestamp")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("RequestedTimestamp")
                        .HasColumnType("TEXT");

                    b.Property<int>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.HasKey("CorrelationId");

                    b.HasIndex("Url")
                        .IsUnique();

                    b.HasIndex("CorrelationId", "Url")
                        .IsUnique();

                    b.ToTable("DownloadVideoState");
                });

            modelBuilder.Entity("Acropolis.Application.Sagas.ExternalMessageRequest.ExternalMessageRequestState", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Channel")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<string>("CurrentState")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<string>("MessageBody")
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<string>("MessageProps")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ReceivedOn")
                        .HasColumnType("TEXT");

                    b.Property<int>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.HasKey("CorrelationId");

                    b.ToTable("ExternalMessageRequestState");
                });

            modelBuilder.Entity("Acropolis.Application.Sagas.ScrapePage.ScrapePageState", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrentState")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<string>("Domain")
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<DateTimeOffset?>("ErrorTimestamp")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("RequestedTimestamp")
                        .HasColumnType("TEXT");

                    b.Property<int>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<DateTimeOffset?>("ScrapedTimestamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("StorageLocation")
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.HasKey("CorrelationId");

                    b.HasIndex("Url")
                        .IsUnique();

                    b.HasIndex("CorrelationId", "Url")
                        .IsUnique();

                    b.ToTable("ScrapePageState");
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.InboxState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Consumed")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ConsumerId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Delivered")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("TEXT");

                    b.Property<long?>("LastSequenceNumber")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("LockId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("TEXT");

                    b.Property<int>("ReceiveCount")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Received")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.HasAlternateKey("MessageId", "ConsumerId");

                    b.HasIndex("Delivered");

                    b.ToTable("InboxStates");
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxMessage", b =>
                {
                    b.Property<long>("SequenceNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<Guid?>("ConversationId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CorrelationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("DestinationAddress")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EnqueueTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("FaultAddress")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("Headers")
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<Guid?>("InboxConsumerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("InboxMessageId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("InitiatorId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("TEXT");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<Guid?>("OutboxId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Properties")
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<Guid?>("RequestId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ResponseAddress")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SentTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("SourceAddress")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("SequenceNumber");

                    b.HasIndex("EnqueueTime");

                    b.HasIndex("ExpirationTime");

                    b.HasIndex("OutboxId", "SequenceNumber")
                        .IsUnique();

                    b.HasIndex("InboxMessageId", "InboxConsumerId", "SequenceNumber")
                        .IsUnique();

                    b.ToTable("OutboxMessage");
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxState", b =>
                {
                    b.Property<Guid>("OutboxId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Delivered")
                        .HasColumnType("TEXT");

                    b.Property<long?>("LastSequenceNumber")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("LockId")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.HasKey("OutboxId");

                    b.HasIndex("Created");

                    b.ToTable("OutboxStates");
                });

            modelBuilder.Entity("Acropolis.Application.Sagas.DownloadVideo.DownloadVideoState", b =>
                {
                    b.OwnsOne("Acropolis.Application.Models.VideoMetaData", "VideoMetaData", b1 =>
                        {
                            b1.Property<Guid>("DownloadVideoStateCorrelationId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Author")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .UseCollation("BINARY");

                            b1.Property<string>("StorageLocation")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .UseCollation("BINARY");

                            b1.Property<string>("VideoId")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .UseCollation("BINARY");

                            b1.Property<string>("VideoTitle")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .UseCollation("BINARY");

                            b1.Property<DateTimeOffset>("VideoUploadTimestamp")
                                .HasColumnType("TEXT");

                            b1.HasKey("DownloadVideoStateCorrelationId");

                            b1.ToTable("DownloadVideoState");

                            b1.WithOwner()
                                .HasForeignKey("DownloadVideoStateCorrelationId");
                        });

                    b.Navigation("VideoMetaData");
                });
#pragma warning restore 612, 618
        }
    }
}
