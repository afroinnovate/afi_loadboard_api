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
        public Task<ResponseDto> Register(ApplicationUser user, string password, string roleName);

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
        public Task<bool> AssignRole(ApplicationUser user, string roleName);

        /// <summary>
        /// Asynchronously removes a role from a user.
        /// </summary>
        /// <param name="user">The user from whom the role should be removed.</param>
        /// <param name="roleName">The name of the role to remove from the user.</param>
        /// <returns>A boolean indicating whether the role was successfully removed from the user.</returns>
        public Task<bool> RemoveRole(ApplicationUser user, string roleName);

        /// <summary>
        /// Asynchronously retrieves a user role based on ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose role should be retrieved.</param>
        /// <returns>The name of the user's role.</returns>
        public Task<List<string>> GetUserRole(string userId);


        /// <summary>
        /// Asynchronously logs a user out.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public Task Logout();

        /// <summary>
        /// Asynchronously resets a user's password using a password reset token.
        /// </summary>
        /// <param name="email">The email of the user resetting their password.</param>
        /// <param name="token">The password reset token.</param>
        /// <param name="newPassword">The new password for the user.</param>
        /// <returns>A ResponseDto indicating the result of the password reset process.</returns>
        public Task<ResponseDto> ResetPasswordAsync(string email, string token, string newPassword);

        /// <summary>
        /// Asynchronously generates a password reset token for a user.
        /// </summary>
        /// <param name="email">The email of the user to generate a reset token for.</param>
        /// <returns>A token that can be used to reset the user's password.</returns>
        public Task<string> GeneratePasswordResetTokenAsync(string email); 
    }
}