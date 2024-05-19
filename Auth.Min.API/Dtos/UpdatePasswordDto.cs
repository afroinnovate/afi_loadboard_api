using System.ComponentModel.DataAnnotations;
namespace Auth.Min.API.Dtos;

/// <summary>
/// Represents a request for resetting a password with validation token.
/// </summary>
public class UpdatePasswordDTO
{
    public string Email { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}

