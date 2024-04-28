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
        public DbSet<User> Users { get; set; } // Unified set for users
        public DbSet<Bid> Bids { get; set; } // Assuming Bidding instead of Bid

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
