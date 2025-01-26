﻿// <auto-generated />
using System;
using Frieght.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Frieght.Api.Infrastructure.Data.Migrations
{
    [DbContext(typeof(FrieghtDbContext))]
    [Migration("20240318043504_UpdateDestinationTypo")]
    partial class UpdateDestinationTypo
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Frieght.Api.Entities.Bid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("BidAmount")
                        .HasColumnType("numeric");

                    b.Property<int>("BidStatus")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("BiddingTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CarrierId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("LoadId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Bids");
                });

            modelBuilder.Entity("Frieght.Api.Entities.Carrier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("AvailableCapacity")
                        .HasColumnType("double precision");

                    b.Property<string>("CompanyEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CompanyPhone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DOTNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EquipmentType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MotorCarrierNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Carriers");
                });

            modelBuilder.Entity("Frieght.Api.Entities.Load", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("AcceptedBidId")
                        .HasColumnType("integer");

                    b.Property<string>("Commodity")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("LoadDetails")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("LoadStatus")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<decimal>("OfferAmount")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)");

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("PickupDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ShipperUserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Weight")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("ShipperUserId");

                    b.ToTable("Loads");
                });

            modelBuilder.Entity("Frieght.Api.Entities.Shipper", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("CompanyName")
                        .HasColumnType("text");

                    b.Property<string>("DOTNumber")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Shipper");
                });

            modelBuilder.Entity("Frieght.Api.Entities.Load", b =>
                {
                    b.HasOne("Frieght.Api.Entities.Shipper", "Shipper")
                        .WithMany("Loads")
                        .HasForeignKey("ShipperUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shipper");
                });

            modelBuilder.Entity("Frieght.Api.Entities.Shipper", b =>
                {
                    b.Navigation("Loads");
                });
#pragma warning restore 612, 618
        }
    }
}
