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
        public DbSet<Load> Loads { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<BusinessProfile> BusinessProfiles { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Vehicle> CarrierVehicle { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
