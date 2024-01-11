using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Auth.Min.API.Dtos;

public class CompleteProfileRequest
{
    [Required]
    public required string Username { get; set; }
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? CompanyName { get; set; }

    public string? DotNumber { get; set; }

    public string? Role { get; set; }
}