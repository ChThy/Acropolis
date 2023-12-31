﻿// <auto-generated />
using System;
using Acropolis.Infrastructure.EfCore.Messenger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Acropolis.Infrastructure.EfCore.Messenger.Migrations
{
    [DbContext(typeof(MessengerDbContext))]
    partial class MessengerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("BINARY")
                .HasAnnotation("ProductVersion", "8.0.0-preview.7.23375.4");

            modelBuilder.Entity("Acropolis.Domain.Messenger.IncomingRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ExternalId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("ProcessedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("RawContent")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .UseCollation("BINARY");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ExternalId");

                    b.HasIndex("ProcessedOn");

                    b.HasIndex("Source");

                    b.HasIndex("Timestamp");

                    b.ToTable("IncomingRequests");
                });

            modelBuilder.Entity("Acropolis.Domain.Messenger.IncomingRequest", b =>
                {
                    b.OwnsOne("Acropolis.Domain.User", "User", b1 =>
                        {
                            b1.Property<Guid>("IncomingRequestId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("ExternalId")
                                .IsRequired()
                                .HasMaxLength(256)
                                .HasColumnType("TEXT")
                                .UseCollation("BINARY");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(256)
                                .HasColumnType("TEXT")
                                .UseCollation("BINARY");

                            b1.HasKey("IncomingRequestId");

                            b1.HasIndex("ExternalId");

                            b1.HasIndex("Name");

                            b1.ToTable("IncomingRequests");

                            b1.WithOwner()
                                .HasForeignKey("IncomingRequestId");
                        });

                    b.Navigation("User")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
