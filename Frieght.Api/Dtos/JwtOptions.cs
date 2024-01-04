namespace Frieght.Api.Dtos;

public class JwtOptions
{
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? SecretKey { get; set; }
    public TimeSpan ValidFor { get; set; }
} 
