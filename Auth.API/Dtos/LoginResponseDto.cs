namespace Auth.API.Dtos
{
    public class LoginResponseDto
    {
        public string? Token { get; set; }
        public UserDto? User { get; set; }
        public bool IsLockedOut { get; set; }
        public bool RequiresTwoFactor { get; set; }
    }
}