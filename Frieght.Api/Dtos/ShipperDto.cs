namespace Frieght.Api.Dtos;

public record ShipperDto
(
    string UserId,
    string Email,
    string FirstName,
    string LastName,
    string? CompanyName,
    string? DOTNumber,  
    string? Phone
);

public record CreateShipperDto
(
    string UserId,
    string Email,
    string FirstName,
    string LastName,
    string? CompanyName,
    string? DOTNumber,
    string? Phone
);

public record UpdateShipperDto
(
    string UserId,
    string Email,
    string FirstName,
    string LastName,
    string? CompanyName,
    string? DOTNumber,
    string? Phone
);
