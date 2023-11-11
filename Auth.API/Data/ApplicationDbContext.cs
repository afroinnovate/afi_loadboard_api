using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Auth.API.Models;
using Microsoft.EntityFrameworkCore;

/// <summary>
///  Identity Framwork core will create all the models needed for the authorization
/// </summary>
namespace Auth.API.Data
{
	public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
		{
			
		}

        public DbSet<ApplicationUser> ApplicationUsers {get; set; }

        //seeding the database with initial data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}