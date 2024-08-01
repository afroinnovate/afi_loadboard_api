using Frieght.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frieght.Api.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.UserId);

            builder.HasIndex(user => user.UserId)
                .IsUnique();

            builder.HasIndex(user => user.Email)
                .IsUnique();

            builder.Property(user => user.Phone)
                .IsRequired(false);

            builder.HasMany(user => user.Loads)
                .WithOne(load => load.Shipper)
                .HasForeignKey(load => load.ShipperUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(user => user.Bids)
                .WithOne(bid => bid.Carrier)
                .HasForeignKey(bid => bid.CarrierId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(user => user.BusinessProfile)
                .WithOne(bp => bp.User)
                .HasForeignKey<BusinessProfile>(bp => bp.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
