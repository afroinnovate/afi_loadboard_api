namespace Auth.API.Dtos
{
    public class LoginResponsDto
    {
        public string? Token { get; set; }
        public UserDto? User { get; set; }
    }
}