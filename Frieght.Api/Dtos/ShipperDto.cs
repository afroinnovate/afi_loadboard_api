namespace Frieght.Api.Dtos;

public record ShipperDto
(
    string UserId,
    string? Email,
    string? FirstName,
    string? LastName,
    string? CompanyName,
    string? DOTNumber  
);
