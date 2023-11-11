using Frieght.Api.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Frieght.Api.Infrastructure.Data.Configurations;


    public class CarrierConfiguration : IEntityTypeConfiguration<Carrier>
    {
        public void Configure(EntityTypeBuilder<Carrier> builder)
        {
            //builder.Property(carrier => carrier.BitAmount)
            //    .HasPrecision(10, 2);
        }

    }
