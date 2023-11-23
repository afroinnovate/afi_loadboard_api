namespace Frieght.Api.Entities;


public class JwtOptions
{
    public string? Secret { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int AccessTokenExpiration { get; set; }
    public int ClockSkew { get; set; } = 5; // Default to 5 seconds
}
