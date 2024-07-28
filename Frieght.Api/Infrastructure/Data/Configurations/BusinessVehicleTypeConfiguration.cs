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
    }
  }
}
