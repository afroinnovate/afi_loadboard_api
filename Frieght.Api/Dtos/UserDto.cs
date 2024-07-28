namespace Frieght.Api.Dtos;
public class UserDto
{
  public required string UserId { get; set; }
  public required string Email { get; set; }
  public string? MiddleName { get; set; }
  public required string FirstName { get; set; }
  public required string LastName { get; set; }
  public string? Phone { get; set; }
  public string? UserType { get; set; } // Shipper or Carrier
  public BusinessProfileDto? BusinessProfile { get; set; }
}