using Frieght.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frieght.Api.Infrastructure.Data.Configurations;

public class BidConfiguration : IEntityTypeConfiguration<Bid>
{
    public void Configure(EntityTypeBuilder<Bid> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.BidAmount)
            .HasPrecision(18, 2); // Configuring precision for monetary value

        // Configuring the relationship between Bid and Load
        builder.HasOne(b => b.Load)
            .WithMany() //Load does not explicitly track its Bids in a collection
            .HasForeignKey(b => b.LoadId)
            .OnDelete(DeleteBehavior.Cascade); // Cascading delete: deleting a Load will delete its associated Bids

        builder.HasOne(b => b.Carrier)
            .WithMany() // User (Carrier) does not explicitly track its Bids in a collection
            .HasForeignKey(b => b.CarrierId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of User when Bids exist

        builder.Property(b => b.BiddingTime)
            .IsRequired();

        builder.HasIndex(b => b.CarrierId); // Index for faster lookup on common queries
    }
}
