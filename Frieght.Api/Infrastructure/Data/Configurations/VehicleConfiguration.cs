namespace Frieght.Api.Infrastructure.Data.Configurations;

using Frieght.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
    builder.HasKey(vehicle => vehicle.Id);

    builder.HasIndex(vehicle => new
    {
      vehicle.LicensePlate,
      vehicle.VIN
    }).IsUnique();

    builder.Property(vehicle => vehicle.Year)
        .IsRequired();

    builder.Property(vehicle => vehicle.Make)
        .IsRequired()
        .HasMaxLength(50);

    builder.Property(vehicle => vehicle.LicensePlate)
        .IsRequired()
        .HasMaxLength(50);

    builder.Property(vehicle => vehicle.VIN)
        .IsRequired()
        .HasMaxLength(50);

    builder.Property(vehicle => vehicle.Color)
        .IsRequired()
        .HasMaxLength(50);

    // Define one-to-many relationship between BusinessProfile and Vehicle
    builder.HasOne(vehicle => vehicle.BusinessProfile)
        .WithMany(bp => bp.CarrierVehicles)
        .HasForeignKey(vehicle => vehicle.BusinessProfileId)
        .OnDelete(DeleteBehavior.Cascade);

    // Define one-to-many relationship between VehicleType and Vehicle
    builder.HasOne(vehicle => vehicle.VehicleType)
        .WithMany()
        .HasForeignKey(vehicle => vehicle.VehicleTypeId)
        .OnDelete(DeleteBehavior.Restrict);
  }
}