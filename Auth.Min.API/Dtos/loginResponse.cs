namespace Auth.Min.API.Dtos
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public bool IsLockedOut { get; set; }
        public bool RequiresTwoFactor { get; set; }
    }
}