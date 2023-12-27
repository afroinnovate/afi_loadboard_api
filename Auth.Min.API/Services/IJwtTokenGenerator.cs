using Auth.Min.API.Models;

namespace Auth.Min.API.Services;

/// <summary>
/// Interface for generating JWT tokens for authenticated users.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Generates a JWT token for the specified user and roles.
    /// </summary>
    /// <param name="user">The user to generate the token for.</param>
    /// <param name="roles">The roles associated with the user.</param>
    /// <returns>The generated JWT token.</returns>
    public string GenerateToken(AppUser user, IEnumerable<string> roles);
}
