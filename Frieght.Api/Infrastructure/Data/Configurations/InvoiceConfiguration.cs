namespace Frieght.Api.Infrastructure.Data.Configurations;

using Frieght.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
  public void Configure(EntityTypeBuilder<Invoice> builder)
  {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .UseIdentityColumn();

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

        builder.Property(invoice => invoice.TransactionId);

        builder.Property(invoice => invoice.TransactionDate);

        builder.Property(invoice => invoice.TransactionStatus);

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

        builder.Property(i => i.PaymentMethodId)
            .IsRequired();

        builder.HasIndex(i => i.PaymentMethodId);

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