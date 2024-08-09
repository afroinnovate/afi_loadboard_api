using Frieght.Api.Entities;
using Frieght.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Frieght.Api.Repositories;

public class BusinessProfileRepository : IBusinessProfileRepository
{
  private readonly FrieghtDbContext context;
  private readonly ILogger<BusinessProfileRepository> _logger;

  public BusinessProfileRepository(FrieghtDbContext context, ILogger<BusinessProfileRepository> logger)
  {
    this.context = context;
    _logger = logger;
  }

  /// <summary>
  /// Get BusinessProfile by UserId
  /// </summary>
  /// <param name="userId"></param>
  /// <returns></returns> <summary>
  /// 
  /// </summary>
  /// <param name="userId"></param>
  /// <returns> a BusinessProfile object if found, otherwise null</returns>
  public async Task<BusinessProfile?> GetBusinessProfileByUserId(string userId)
  {
    _logger.LogInformation("Attempting to retrieve BusinessProfile for UserId: {UserId}", userId);
    try
    {
      var profile = await context.BusinessProfiles
          .Include(bp => bp.CarrierVehicles)
            .ThenInclude(bvt => bvt.VehicleType)
          .AsNoTracking()
          .FirstOrDefaultAsync(bp => bp.UserId == userId);

      if (profile == null)
      {
        _logger.LogWarning("No BusinessProfile found for UserId: {UserId}", userId);
        return null;
      }

      _logger.LogInformation("BusinessProfile retrieved successfully for UserId: {UserId}", userId);
      return profile;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error occurred while retrieving BusinessProfile for UserId: {UserId}", userId);
      throw;
    }
  }
  /// <summary>
  /// Update BusinessProfile
  /// </summary>
  /// <param name="businessProfile"></param>
  /// <returns></returns> <summary>
  /// 
  /// </summary>
  /// <param name="businessProfile"></param>
  /// <returns>Return void</returns>
  public async Task UpdateBusinessProfile(BusinessProfile businessProfile)
  {
    _logger.LogInformation("Updating BusinessProfile for UserId: {UserId}", businessProfile.UserId);
    try
    {
      context.BusinessProfiles.Update(businessProfile);
      await context.SaveChangesAsync();
      _logger.LogInformation("BusinessProfile for UserId: {UserId} updated successfully", businessProfile.UserId);
    }
    catch (DbUpdateException ex)
    {
      _logger.LogError(ex, "Database update error while updating BusinessProfile for UserId: {UserId}", businessProfile.UserId);
      throw;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error updating BusinessProfile for UserId: {UserId}", businessProfile.UserId);
      throw;
    }
  }

}