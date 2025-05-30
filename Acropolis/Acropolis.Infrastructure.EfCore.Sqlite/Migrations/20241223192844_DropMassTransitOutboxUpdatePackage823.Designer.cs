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
    [Migration("20241223192844_DropMassTransitOutboxUpdatePackage823")]
    partial class DropMassTransitOutboxUpdatePackage823
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
