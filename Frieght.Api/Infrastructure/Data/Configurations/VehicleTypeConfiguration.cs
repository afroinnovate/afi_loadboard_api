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

      builder.Property(vt => vt.Name)
          .IsRequired()
          .HasMaxLength(50);

      builder.HasMany(vt => vt.BusinessVehicleTypes)
          .WithOne(bvt => bvt.VehicleType)
          .HasForeignKey(bvt => bvt.VehicleTypeId)
          .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
