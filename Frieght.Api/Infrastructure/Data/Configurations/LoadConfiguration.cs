


using Frieght.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frieght.Api.Infrastructure.Data.Configurations;

public class LoadConfiguration: IEntityTypeConfiguration<Load>
{
    public void Configure(EntityTypeBuilder<Load> builder)
    {
        builder.Property(load => load.OfferAmount)
            .HasPrecision(10, 2);

        builder.Property(load => load.Origin)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(load => load.Destination)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(load => load.LoadStatus)
            .HasMaxLength(20)
            .IsRequired();

        // Define primary key if not conventionally done by EF Core
        builder.HasKey(load => load.LoadId);
        
        // Add an index on Origin and Destination for faster searches
        builder.HasIndex(load => new { load.Origin, load.Destination });

        // Configure relationships if not done in the DbContext
        builder.HasOne(load => load.Shipper)
            .WithMany()
            .HasForeignKey(load => load.ShipperUserId)
            .OnDelete(DeleteBehavior.Restrict);  // Prevent deletion of User if they have posted Loads
    }

}
