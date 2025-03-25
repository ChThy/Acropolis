﻿// <auto-generated />
using System;
using Acropolis.Infrastructure.EfCore;
using Acropolis.Infrastructure.EfCore.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Acropolis.Infrastructure.EfCore.Messenger.Migrations
{
    [DbContext(typeof(SqliteAppDbContext))]
    [Migration("20241228192746_AddDenormalizedTables")]
    partial class AddDenormalizedTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("BINARY")
                .HasAnnotation("ProductVersion", "9.0.0");

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

            modelBuilder.Entity("Acropolis.Domain.DownloadedVideos.DownloadedVideo", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.HasKey("Id");

                    b.ToTable("DownloadedVideos");
                });

            modelBuilder.Entity("Acropolis.Domain.Resource", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedTimestamp")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("DownloadedVideoId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ScrapedPageId")
                        .HasColumnType("TEXT");

                    b.Property<string>("StorageLocation")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<int>("Views")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DownloadedVideoId");

                    b.HasIndex("ScrapedPageId");

                    b.ToTable("Resource");
                });

            modelBuilder.Entity("Acropolis.Domain.ScrapedPages.ScrapedPage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.HasKey("Id");

                    b.ToTable("ScrapedPages");
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

            modelBuilder.Entity("Acropolis.Domain.DownloadedVideos.DownloadedVideo", b =>
                {
                    b.OwnsOne("Acropolis.Domain.DownloadedVideos.VideoMetaData", "MetaData", b1 =>
                        {
                            b1.Property<Guid>("DownloadedVideoId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Author")
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

                            b1.HasKey("DownloadedVideoId");

                            b1.ToTable("DownloadedVideos");

                            b1.WithOwner()
                                .HasForeignKey("DownloadedVideoId");
                        });

                    b.Navigation("MetaData")
                        .IsRequired();
                });

            modelBuilder.Entity("Acropolis.Domain.Resource", b =>
                {
                    b.HasOne("Acropolis.Domain.DownloadedVideos.DownloadedVideo", null)
                        .WithMany("Resources")
                        .HasForeignKey("DownloadedVideoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Acropolis.Domain.ScrapedPages.ScrapedPage", null)
                        .WithMany("Resources")
                        .HasForeignKey("ScrapedPageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Acropolis.Domain.ScrapedPages.ScrapedPage", b =>
                {
                    b.OwnsOne("Acropolis.Domain.ScrapedPages.PageMetaData", "MetaData", b1 =>
                        {
                            b1.Property<Guid>("ScrapedPageId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Domain")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .UseCollation("BINARY");

                            b1.Property<string>("PageTitle")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .UseCollation("BINARY");

                            b1.HasKey("ScrapedPageId");

                            b1.ToTable("ScrapedPages");

                            b1.WithOwner()
                                .HasForeignKey("ScrapedPageId");
                        });

                    b.Navigation("MetaData")
                        .IsRequired();
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxMessage", b =>
                {
                    b.HasOne("MassTransit.EntityFrameworkCoreIntegration.OutboxState", null)
                        .WithMany()
                        .HasForeignKey("OutboxId");

                    b.HasOne("MassTransit.EntityFrameworkCoreIntegration.InboxState", null)
                        .WithMany()
                        .HasForeignKey("InboxMessageId", "InboxConsumerId")
                        .HasPrincipalKey("MessageId", "ConsumerId");
                });

            modelBuilder.Entity("Acropolis.Domain.DownloadedVideos.DownloadedVideo", b =>
                {
                    b.Navigation("Resources");
                });

            modelBuilder.Entity("Acropolis.Domain.ScrapedPages.ScrapedPage", b =>
                {
                    b.Navigation("Resources");
                });
#pragma warning restore 612, 618
        }
    }
}
