using Frieght.Api.Entities;
using Frieght.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Frieght.Api.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly FrieghtDbContext context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(FrieghtDbContext context, ILogger<UserRepository> logger)
    {
      this.context = context;
      _logger = logger;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
      return await context.Users
          .Include(u => u.BusinessProfile)
          .ThenInclude(bp => bp.BusinessVehicleTypes)
          .ToListAsync();
    }

    public async Task<User?> GetUser(string id)
    {
      return await context.Users
          .Include(u => u.BusinessProfile)
          .ThenInclude(bp => bp.BusinessVehicleTypes)
          .FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task CreateUser(User user)
    {
      context.Users.Add(user);
      await context.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
      context.Users.Update(user);
      await context.SaveChangesAsync();
    }

    public async Task DeleteUser(User user)
    {
      context.Users.Remove(user);
      await context.SaveChangesAsync();
    }
  }
}
