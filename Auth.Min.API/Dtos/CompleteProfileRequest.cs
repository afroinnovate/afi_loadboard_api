using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Auth.Min.API.Dtos;

public class CompleteProfileRequest
{
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string FirstName { get; set; }

     [Required]
    public required string LastName { get; set; }

    public string? CompanyName { get; set; }

    public string? DotNumber { get; set; }
    
    [Required]
    public required string Role { get; set; }
}