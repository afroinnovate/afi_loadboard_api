using Auth.API.Dtos;

namespace Auth.API.Services.IServices
{
    /// <summary>
    /// Interface for authentication service.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registerationRequestDto">The registration request data transfer object.</param>
        /// <returns>The registered user's ID.</returns>
        public Task<ResponseDto> Register(RegistrationRequestDto registerationRequestDto, string roleNames);

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="loginRequestDto">The login request data transfer object.</param>
        /// <returns>The login response data transfer object.</returns>
        public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto, string loginType);

        /// <summary>
        /// logs out a user.
        ///  </summary>
        /// <returns>The logout response data transfer object.</returns>
        public Task logout();

        /// <summary>
        /// Assigns a role to a user.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="roleName">The name of the role to assign.</param>
        /// <returns>True if the role was assigned successfully, false otherwise.</returns>
        public Task<bool> AssignRole(string userId, string roleName);

        /// <summary>
        /// Assigns a role to a user.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="roleName">The name of the role to assign.</param>
        /// <returns>True if the role was assigned successfully, false otherwise.</returns>
        public Task<bool> RemoveRole(string userId, string roleName);

        /// <summary>
        /// Asynchronously generates a password reset token for a user.
        /// </summary>
        /// <param name="email">The email of the user to generate a reset token for.</param>
        /// <returns>A token that can be used to reset the user's password.</returns>
        public Task<string> GeneratePasswordResetTokenAsync(string email);

        /// <summary>
        /// Asynchronously resets a user's password using a password reset token.
        /// </summary>
        /// <param name="email">The email of the user resetting their password.</param>
        /// <param name="token">The password reset token.</param>
        /// <param name="newPassword">The new password for the user.</param>
        /// <returns>A ResponseDto indicating the result of the password reset process.</returns>
        public Task<ResponseDto> ResetPasswordAsync(string email, string token, string newPassword);
    }
}