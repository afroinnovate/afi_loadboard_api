using Frieght.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frieght.Api.Infrastructure.Data.Configurations
{
  public class VehicleTypeConfiguration : IEntityTypeConfiguration<VehicleType>
  {
    public void Configure(EntityTypeBuilder<VehicleType> builder)
    {
      builder.HasKey(vt => vt.Id);

      // add index for faster lookup and enforce uniqueness
      builder.HasIndex(vt => new{
        vt.Id,
        vt.Name
      }).IsUnique();
  
      builder.Property(vt => vt.Name)
          .IsRequired()
          .HasMaxLength(50);
      
      // one vehicle type can have many vehicles but a vehicle can only belong to one vehicle type
      builder.HasMany(vt => vt.Vehicles)
          .WithOne(v => v.VehicleType)
          .HasForeignKey(v => v.VehicleTypeId)
          .OnDelete(DeleteBehavior.Restrict);
    }
  }
}
