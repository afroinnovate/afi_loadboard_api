using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Auth.Min.API.Dtos;

public class CompleteProfileRequest
{
    public required string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public required string? LastName { get; set; }

    public required string? Role { get; set; }

    public required string Email { get; set; }

    public required string PhoneNumber { get; set; }
}