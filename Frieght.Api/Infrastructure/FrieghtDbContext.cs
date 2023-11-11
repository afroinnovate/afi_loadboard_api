using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Frieght.Api.Entities;

namespace Frieght.Api.Infrastructure
{
    public class FrieghtDbContext : DbContext
    {
        public FrieghtDbContext(DbContextOptions<FrieghtDbContext> options) : base(options)
        {

        }
        public DbSet<Load> Loads => Set<Load>();
        public DbSet<Carrier> Carriers => Set<Carrier>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
