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
              .ThenInclude(bp => bp.CarrierVehicles)
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
              .ThenInclude(bp => bp.CarrierVehicles)
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
              .ThenInclude(bp => bp.CarrierVehicles)
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

        if (user.BusinessProfile != null && user.BusinessProfile.CarrierVehicles != null)
        {
          foreach (var vehicle in user.BusinessProfile.CarrierVehicles)
          {
            // Check if the VehicleType exists or create a new one
            var vehicleType = await context.VehicleTypes.FirstOrDefaultAsync(vt => vt.Name == vehicle.Name);
            if (vehicleType == null)
            {
              _logger.LogInformation("VehicleType '{VehicleName}' not found. Creating a new VehicleType.", vehicle.Name);
              vehicleType = new VehicleType
              {
                Name = vehicle.Name
              };
              
              context.VehicleTypes.Add(vehicleType);
              await context.SaveChangesAsync();  // Save to generate the VehicleTypeId
              _logger.LogInformation("VehicleType '{VehicleName}' created with Id: {VehicleTypeId}", vehicle.Name, vehicleType.Id);
            }


            // Assign the VehicleTypeId to the vehicle
            vehicle.VehicleTypeId = vehicleType.Id;
            _logger.LogInformation("Creating new Carrier Vehicle with UserId: {UserId} and vehichle Type {type}", user.UserId, vehicle.Name);
            context.CarrierVehicle.Add(vehicle);
          }
        }

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
