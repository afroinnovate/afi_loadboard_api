namespace Auth.Min.API.Models
{
    public class JwtOptions
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? SecretKey { get; set; }
        public TimeSpan ValidFor { get; set; }
        public int ClockSkew { get; set; } = 5; // Default to 5 seconds
    } 
}