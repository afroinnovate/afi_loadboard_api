namespace Frieght.Api.Infrastructure.Data.Configurations;

using Frieght.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
  public void Configure(EntityTypeBuilder<Invoice> builder)
  {
    builder.HasKey(invoice => invoice.Id);

        builder.Property(invoice => invoice.Id)
            .ValueGeneratedOnAdd();

        builder.HasIndex(invoice => invoice.InvoiceNumber)
        .IsUnique();

    builder.Property(invoice => invoice.InvoiceNumber)
        .IsRequired()
        .HasMaxLength(50);

        builder.Property(invoice => invoice.CarrierId)
            .IsRequired();

        builder.HasIndex(invoice => invoice.CarrierId);

        builder.Property(invoice => invoice.Status)
        .IsRequired()
        .HasMaxLength(20);

    builder.Property(invoice => invoice.TransactionId)
        .IsRequired();

        builder.Property(invoice => invoice.Note)
            .HasMaxLength(500);

    // Money-related properties precision configuration
    builder.Property(invoice => invoice.AmountDue)
        .HasPrecision(18, 2);

    builder.Property(invoice => invoice.TotalAmount)
        .HasPrecision(18, 2);

    builder.Property(invoice => invoice.TotalVat)
        .HasPrecision(18, 2);

    builder.Property(invoice => invoice.Withholding)
        .HasPrecision(18, 2);

    builder.Property(invoice => invoice.ServiceFees)
        .HasPrecision(18, 2);

        builder.Property(invoice => invoice.PaymentMethodId)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(invoice => invoice.PaymentMethodId);

    // Add configurations for carrier details
    builder.Property(invoice => invoice.CarrierName)
        .HasMaxLength(100);
    
    builder.Property(invoice => invoice.CarrierEmail)
        .HasMaxLength(255);
    
    builder.Property(invoice => invoice.CarrierPhone)
        .HasMaxLength(20);
    
    builder.Property(invoice => invoice.CarrierBusinessName)
        .HasMaxLength(255);
  }
}