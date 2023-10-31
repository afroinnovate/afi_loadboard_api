using Auth.API.Dtos;
using Auth.API.Models;

namespace Auth.API.Repository
{
    /// <summary>
    /// Provides a contract for CRUD operations specific to the ApplicationUser entity, 
    /// as well as additional operations related to user management.
    /// </summary>
    public interface IUserRepository : IBaseRepository<ApplicationUser>
    {
        /// <summary>
        /// Asynchronously retrieves a user based on the username.
        /// </summary>
        /// <param name="userName">The username of the user to retrieve.</param>
        /// <returns>An instance of the UserDto representing the user, or null if no user matches the username.</returns>
        public Task<UserDto> GetByUserNameAsync(string userName);

        /// <summary>
        /// Asynchronously registers a new user.
        /// </summary>
        /// <param name="registrationRequestDto">The data transfer object containing user registration details.</param>
        /// <returns>A string message indicating the result of the registration process.</returns>
        public Task<string> Register(RegistrationRequestDto registrationRequestDto);

        /// <summary>
        /// Asynchronously logs a user in.
        /// </summary>
        /// <param name="loginRequestDto">The data transfer object containing user login details.</param>
        /// <returns>An instance of the LoginResponseDto containing the login result and related data.</returns>
        public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

        /// <summary>
        /// Asynchronously assigns a role to a user.
        /// </summary>
        /// <param name="userId">The ID of the user to whom the role should be assigned.</param>
        /// <param name="roleName">The name of the role to assign to the user.</param>
        /// <returns>A boolean indicating whether the role was successfully assigned to the user.</returns>
        public Task<bool> AssignRole(string userId, string roleName);
    }
}