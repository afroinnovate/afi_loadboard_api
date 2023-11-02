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
        /// Assigns a role to a user.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="roleName">The name of the role to assign.</param>
        /// <returns>True if the role was assigned successfully, false otherwise.</returns>
        public Task<bool> AssignRole(string userId, string roleName);
    }
}