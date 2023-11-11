


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
    }

}
