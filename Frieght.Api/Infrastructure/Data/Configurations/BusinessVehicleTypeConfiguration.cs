using Frieght.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frieght.Api.Infrastructure.Data.Configurations
{
  public class BusinessVehicleTypeConfiguration : IEntityTypeConfiguration<BusinessVehicleType>
  {
    public void Configure(EntityTypeBuilder<BusinessVehicleType> builder)
    {
      builder.HasKey(bvt => new { bvt.BusinessProfileId, bvt.VehicleTypeId });

      builder.Property(bvt => bvt.Quantity)
          .IsRequired()
          .HasDefaultValue(0);

      builder.HasOne(bvt => bvt.BusinessProfile)
          .WithMany(bp => bp.BusinessVehicleTypes)
          .HasForeignKey(bvt => bvt.BusinessProfileId)
          .OnDelete(DeleteBehavior.Cascade);

      builder.HasOne(bvt => bvt.VehicleType)
          .WithMany(vt => vt.BusinessVehicleTypes)
          .HasForeignKey(bvt => bvt.VehicleTypeId)
          .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
