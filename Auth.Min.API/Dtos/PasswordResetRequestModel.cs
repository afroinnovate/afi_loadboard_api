using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents a request for initiating a password reset process.
/// </summary>
public class PasswordResetRequestModelDTO
{
    /// <summary>
    /// Gets or sets the email of the user requesting a password reset.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
