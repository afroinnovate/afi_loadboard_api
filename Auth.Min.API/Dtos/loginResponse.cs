using Auth.Min.API.Models;

namespace Auth.Min.API.Dtos
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public bool IsLockedOut { get; set; }
        public bool RequiresTwoFactor { get; set; }
        public UserDto? User { get; set; }
    }
}