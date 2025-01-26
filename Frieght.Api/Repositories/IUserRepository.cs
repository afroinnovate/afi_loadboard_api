using Frieght.Api.Entities;

namespace Frieght.Api.Repositories;

public interface IUserRepository
{
  Task<IEnumerable<User>> GetUsers();
  Task<User?> GetUser(string id);
  public Task<IEnumerable<User?>> GetUserByUserType(string userType);
  Task CreateUser(User user);
  Task UpdateUser(User user);
  Task DeleteUser(User user);
}

