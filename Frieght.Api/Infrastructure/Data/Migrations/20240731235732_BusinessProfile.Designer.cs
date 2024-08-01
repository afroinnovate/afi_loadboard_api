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
    [Migration("20240731235732_BusinessProfile")]
    partial class BusinessProfile
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BusinessVehicleType", b =>
                {
                    b.Property<int>("BusinessProfileId")
                        .HasColumnType("integer");

                    b.Property<int>("VehicleTypeId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("BusinessProfileId", "VehicleTypeId");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("BusinessVehicleTypes");
                });

            modelBuilder.Entity("Frieght.Api.Entities.Bid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("BidAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");

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
                        .HasColumnType("text");

                    b.Property<string>("UserType")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CarrierId");

                    b.HasIndex("LoadId");

                    b.ToTable("Bids");
                });

            modelBuilder.Entity("Frieght.Api.Entities.BusinessProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double?>("AvailableCapacity")
                        .HasPrecision(18, 2)
                        .HasColumnType("double precision");

                    b.Property<string>("BusinessRegistrationNumber")
                        .HasColumnType("text");

                    b.Property<string>("BusinessType")
                        .HasColumnType("text");

                    b.Property<int?>("CarrierRole")
                        .HasColumnType("integer");

                    b.Property<string>("CompanyName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("DOTNumber")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("EquipmentType")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("IDCardOrDriverLicenceNumber")
                        .HasColumnType("text");

                    b.Property<string>("InsuranceName")
                        .HasColumnType("text");

                    b.Property<string>("MotorCarrierNumber")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int?>("ShipperRole")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("BusinessProfiles");
                });

            modelBuilder.Entity("Frieght.Api.Entities.Load", b =>
                {
                    b.Property<int>("LoadId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LoadId"));

                    b.Property<string>("Commodity")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<DateTime>("CreatedAt")
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

                    b.Property<DateTime?>("ModifiedAt")
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

                    b.Property<double>("Weight")
                        .HasColumnType("double precision");

                    b.HasKey("LoadId");

                    b.HasIndex("ShipperUserId");

                    b.HasIndex("Origin", "Destination");

                    b.ToTable("Loads");
                });

            modelBuilder.Entity("Frieght.Api.Entities.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Frieght.Api.Entities.VehicleType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Color")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool?>("HasInspection")
                        .HasColumnType("boolean");

                    b.Property<bool?>("HasInsurance")
                        .HasColumnType("boolean");

                    b.Property<bool?>("HasRegistration")
                        .HasColumnType("boolean");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("LicensePlate")
                        .HasColumnType("text");

                    b.Property<string>("Make")
                        .HasColumnType("text");

                    b.Property<string>("Model")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("VIN")
                        .HasColumnType("text");

                    b.Property<string>("Year")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("VehicleTypes");
                });

            modelBuilder.Entity("BusinessVehicleType", b =>
                {
                    b.HasOne("Frieght.Api.Entities.BusinessProfile", "BusinessProfile")
                        .WithMany("BusinessVehicleTypes")
                        .HasForeignKey("BusinessProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Frieght.Api.Entities.VehicleType", "VehicleType")
                        .WithMany("BusinessVehicleTypes")
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BusinessProfile");

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("Frieght.Api.Entities.Bid", b =>
                {
                    b.HasOne("Frieght.Api.Entities.User", "Carrier")
                        .WithMany("Bids")
                        .HasForeignKey("CarrierId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Frieght.Api.Entities.Load", "Load")
                        .WithMany()
                        .HasForeignKey("LoadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Carrier");

                    b.Navigation("Load");
                });

            modelBuilder.Entity("Frieght.Api.Entities.BusinessProfile", b =>
                {
                    b.HasOne("Frieght.Api.Entities.User", "User")
                        .WithOne("BusinessProfile")
                        .HasForeignKey("Frieght.Api.Entities.BusinessProfile", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Frieght.Api.Entities.Load", b =>
                {
                    b.HasOne("Frieght.Api.Entities.User", "Shipper")
                        .WithMany("Loads")
                        .HasForeignKey("ShipperUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Shipper");
                });

            modelBuilder.Entity("Frieght.Api.Entities.BusinessProfile", b =>
                {
                    b.Navigation("BusinessVehicleTypes");
                });

            modelBuilder.Entity("Frieght.Api.Entities.User", b =>
                {
                    b.Navigation("Bids");

                    b.Navigation("BusinessProfile");

                    b.Navigation("Loads");
                });

            modelBuilder.Entity("Frieght.Api.Entities.VehicleType", b =>
                {
                    b.Navigation("BusinessVehicleTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
