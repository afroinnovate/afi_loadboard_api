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

            builder.Property(bp => bp.BusinessRegistrationNumber)
                .HasMaxLength(100);

            builder.Property(bp => bp.IDCardOrDriverLicenceNumber)
                .HasMaxLength(50);

            builder.Property(bp => bp.InsuranceName)
                .HasMaxLength(255);

            builder.Property(bp => bp.BusinessType)
                .HasMaxLength(100);

            // Define one-to-one relationship between User and BusinessProfile
            builder.HasOne<User>()
                .WithOne()
                .HasForeignKey<BusinessProfile>(bp => bp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define one-to-many relationship between BusinessProfile and Vehicle
            builder.HasMany(bp => bp.CarrierVehicles)
                .WithOne(v => v.BusinessProfile)
                .HasForeignKey(v => v.BusinessProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
