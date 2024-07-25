using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Auth.Min.API.Models;
using Auth.Min.API.Dtos;

namespace Auth.Min.API.Services;

public class UserService : IUserService
{
  public class LoggerCategory
  {
    // This csass is used to define the category for the logger
  }
  private readonly UserManager<AppUser> _userManager;
  private readonly ILogger<LoggerCategory> _logger;

  public UserService(UserManager<AppUser> userManager, ILogger<LoggerCategory> logger)
  {
    _logger = logger;
    _userManager = userManager;
  }

  public async Task<UserDto> GetUser(string id)
  {
    try
    {
      var user = await _userManager.FindByIdAsync(id);
      if(user == null)
      {
        return await Task.FromResult<UserDto>(new UserDto());
      }
      // For each user, get their roles
      var roles = await _userManager.GetRolesAsync(user);
      return user.ToUserDto(roles);
    }
    catch (Exception ex){
      _logger.LogError(ex, "An error occurred while getting user by id {Id}", id);
      throw new Exception($"An error occurred while getting user by id: {ex.Message}");
    }
  }

  public async Task<IdentityResult> UpdateUser(AppUser user)
  {
    try
    {
      _logger.LogInformation("Updating user");
      return await _userManager.UpdateAsync(user);
    }
    catch (Exception ex)
    {
      _logger.LogError("An error occurred while updating user with error: {message}", ex.Message);
      throw new Exception($"An error occurred while updating user with error: {ex.Message}");
    }
  }
}

