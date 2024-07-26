using Auth.Min.API.Dtos;
namespace Auth.Min.API.Models;

public static class EntitiesExtensions
{
  public static UserDto ToUserDto(this AppUser user, IEnumerable<string> roles)
  {
    return new UserDto
    {
      Id = user.Id,
      UserName = user.UserName,
      Email = user.Email,
      FirstName = user.FirstName,
      MiddleName = user.MiddleName,
      LastName = user.LastName,
      Roles = roles,
      PhoneNumber = user.PhoneNumber,
      Confirmed = user.Confirmed,
      Status = user.Status,
      UserType = user.UserType
    };
  }

  public static AppUser ToAppUser(this UserDto user)
  {
    return new AppUser
    {
      FirstName = user.FirstName,
      MiddleName = user.MiddleName,
      LastName = user.LastName,
      Email = user.Email,
      PhoneNumber = user.PhoneNumber,
      UserName = user.Email,
      Confirmed = user.Confirmed,
      Status = user.Status,
      UserType = user.UserType
    };
  }
}