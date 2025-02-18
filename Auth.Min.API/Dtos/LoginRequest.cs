using System.ComponentModel.DataAnnotations;

namespace Auth.Min.API.Models;

public class LoginRequest
{
    [Required]
    public required string Username { get; set; }
    
    [Required]
    public required string Password { get; set; }
}
