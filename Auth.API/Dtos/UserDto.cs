namespace Auth.API.Dtos 
{
    public class UserDto 
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? MiddleName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public DateTime DateRegistered { get; set; }
        public DateTime DateLoggedIn { get; set; }
    }
}