using Frieght.Api.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Frieght.Api.Infrastructure.Data.Configurations;


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
            builder.Property(user => user.MotorCarrierNumber).IsRequired(false);
            builder.Property(user => user.DOTNumber).IsRequired(false);
            builder.Property(user => user.EquipmentType).IsRequired(false);
            builder.Property(user => user.AvailableCapacity).IsRequired(false);
            builder.Property(user => user.CompanyName).IsRequired(false);

            // Relationships
            // If a User can be both a Shipper and a Carrier, you might have Load and Bidding
            // For example, assuming a User can have multiple Loads they have posted
            builder.HasMany<Load>(user => user.Loads)
                .WithOne(load => load.Shipper)
                .HasForeignKey(load => load.ShipperUserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if there are related loads

            // A User can have multiple Biddings they have made
            builder.HasMany<Bid>(user => user.Bids)
                .WithOne(bid => bid.Carrier)
                .HasForeignKey(bid => bid.CarrierId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if there are related biddings
                
        }

    }
