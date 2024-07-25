namespace Auth.Min.API.Dtos;

public record UpdateUserDto(
  string? FirstName, 
  string? MiddleName,
  string? LastName, 
  string? Email, 
  string? PhoneNumber, 
  string? Role
);
