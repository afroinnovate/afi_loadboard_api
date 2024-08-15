using System.ComponentModel.DataAnnotations;
namespace Auth.Min.API.Dtos;

/// <summary>
/// Represents a request for resetting a password with validation token.
/// </summary>
public class UpdatePasswordDTO
{
    public required string Email { get; set; }
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}

