using Frieght.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frieght.Api.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Set the primary key
            builder.HasKey(user => user.UserId);

            // UserId is a unique identifier like username or email
            builder.HasIndex(user => user.UserId)
                .IsUnique();

            // Email as a unique field
            builder.HasIndex(user => user.Email)
                .IsUnique();

            // Configuring optional properties
            builder.Property(user => user.Phone).IsRequired(false);

            // Relationships
            builder.HasMany(user => user.Loads)
                .WithOne(load => load.Shipper)
                .HasForeignKey(load => load.ShipperUserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if there are related loads

            builder.HasMany(user => user.Bids)
                .WithOne(bid => bid.Carrier)
                .HasForeignKey(bid => bid.CarrierId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if there are related biddings

            // One-to-one relationship with BusinessProfile
            builder.HasOne(user => user.BusinessProfile)
                .WithOne(bp => bp.User)
                .HasForeignKey<BusinessProfile>(bp => bp.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete the BusinessProfile if User is deleted
        }
    }
}
