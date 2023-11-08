using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Auth.Min.API.Models;
using Microsoft.EntityFrameworkCore;

/// <summary>
///  Identity Framwork core will create all the models needed for the authorization
/// </summary>
namespace Auth.Min.API.Data
{
	public class AppDbContext: IdentityDbContext<AppUser>
	{
		public  AppDbContext(DbContextOptions<AppDbContext> options): base(options)
		{
			
		}
    }
}