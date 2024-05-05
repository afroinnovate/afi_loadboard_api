using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents a request for resetting a password with validation token.
/// </summary>
public class ResetPasswordModelDTO
{
    /// <summary>
    /// Gets or sets the email of the user whose password is being reset.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the password reset token provided via email.
    /// </summary>
    [Required]
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the new password for the user.
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
}
