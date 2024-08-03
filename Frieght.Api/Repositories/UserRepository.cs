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
      try
      {
        _logger.LogInformation("Retrieving all users");
        return await context.Users
            .Include(u => u.BusinessProfile)
              .ThenInclude(bp => bp.BusinessVehicleTypes)
            .AsNoTracking()
            .ToListAsync();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error occurred while retrieving all users");
        throw;
      }
    }

    public async Task<User?> GetUser(string id)
    {
      try
      {
        _logger.LogInformation("Retrieving user with ID: {UserId}", id);
        return await context.Users
            .Include(u => u.BusinessProfile)
              .ThenInclude(bp => bp.BusinessVehicleTypes)
            .FirstOrDefaultAsync(u => u.UserId == id);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error occurred while retrieving user with ID: {UserId}", id);
        throw;
      }
    }

    public async Task<IEnumerable<User?>> GetUserByUserType(string userType)
    {
      try
      {
        _logger.LogInformation("Retrieving user with UserType: {UserType}", userType);
        return await context.Users
            .Include(u => u.BusinessProfile)
              .ThenInclude(bp => bp.BusinessVehicleTypes)
            .Where(u => u.UserType == userType)
            .AsNoTracking()
            .ToListAsync();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error occurred while retrieving user with UserType: {UserType}", userType);
        throw;
      }
    }

    public async Task CreateUser(User user)
    {
      try
      {
        await context.Database.BeginTransactionAsync();

        _logger.LogInformation("Creating user with ID: {UserId}", user.UserId);
        context.Users.Add(user);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
        _logger.LogInformation("User created successfully with ID: {UserId}", user.UserId);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error occurred while creating user with ID: {UserId}", user.UserId);
        throw;
      }
    }

    public async Task UpdateUser(User user)
    {
      try
      {
        await context.Database.BeginTransactionAsync();

        _logger.LogInformation("Updating user with ID: {UserId}", user.UserId);
        context.Users.Update(user);
        await context.SaveChangesAsync();
        await context.Database.CommitTransactionAsync();
        _logger.LogInformation("User updated successfully with ID: {UserId}", user.UserId);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error occurred while updating user with ID: {UserId}", user.UserId);
        throw;
      }
    }

    public async Task DeleteUser(User user)
    {
      try
      {
        _logger.LogInformation("Deleting user with ID: {UserId}", user.UserId);
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        _logger.LogInformation("User deleted successfully with ID: {UserId}", user.UserId);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error occurred while deleting user with ID: {UserId}", user.UserId);
        throw;
      }
    }
  }
}
