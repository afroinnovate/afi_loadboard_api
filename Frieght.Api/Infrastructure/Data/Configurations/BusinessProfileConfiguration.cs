using Frieght.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frieght.Api.Infrastructure.Data.Configurations
{
    public class BusinessProfileConfiguration : IEntityTypeConfiguration<BusinessProfile>
    {
        public void Configure(EntityTypeBuilder<BusinessProfile> builder)
        {
            builder.HasKey(bp => bp.Id);

            builder.Property(bp => bp.MotorCarrierNumber)
                .HasMaxLength(50);

            builder.Property(bp => bp.DOTNumber)
                .HasMaxLength(50);

            builder.Property(bp => bp.EquipmentType)
                .HasMaxLength(100);

            builder.Property(bp => bp.AvailableCapacity)
                .HasPrecision(18, 2);

            builder.Property(bp => bp.CompanyName)
                .HasMaxLength(255);

            builder.HasOne(bp => bp.User)
                .WithOne(u => u.BusinessProfile)
                .HasForeignKey<BusinessProfile>(bp => bp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(bp => bp.BusinessVehicleTypes)
                .WithOne(bvt => bvt.BusinessProfile)
                .HasForeignKey(bvt => bvt.BusinessProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
