using System.Threading.Tasks;
using Auth.Min.API.Dtos;
using Auth.Min.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth.Min.API.Services;
public interface IUserService
{
  Task<UserDto> GetUser(string id);
  Task<IdentityResult> UpdateUser(AppUser user);
}

