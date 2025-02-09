using Frieght.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frieght.Api.Infrastructure.Data.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
  public void Configure(EntityTypeBuilder<PaymentMethod> builder)
  {
    builder.HasKey(p => p.Id);

        // Configure ID as auto-incrementing
        builder.Property(p => p.Id)
            .UseIdentityColumn();

        // Ensure PaymentMethodId is unique
        builder.HasIndex(p => p.PaymentMethodId)
            .IsUnique();

        builder.Property(p => p.PaymentType)
        .IsRequired()
        .HasMaxLength(50);

    // Optional fields with appropriate max lengths
    builder.Property(p => p.BankName)
        .HasMaxLength(100);

    builder.Property(p => p.BankAccount)
        .HasMaxLength(50);

    builder.Property(p => p.AccountHolderName)
        .HasMaxLength(100);

    builder.Property(p => p.PhoneNumber)
        .HasMaxLength(20);

    builder.Property(p => p.CardMethod)
        .HasMaxLength(50);

        builder.HasKey(p => p.PaymentMethodId);

        builder.Property(p => p.CardType)
    .HasMaxLength(50);

        builder.Property(p => p.LastFourDigits)
        .HasMaxLength(4);

    builder.Property(p => p.BillingAddress)
        .HasMaxLength(200);

    // Update CarrierId configuration
    builder.Property(p => p.CarrierId)
        .IsRequired();

    // Optional: Add an index on CarrierId since we'll be querying by it
    builder.HasIndex(p => p.CarrierId);
  }
}