using Frieght.Api.Enuns;

namespace Frieght.Api.Dtos;

public record ShipperDto
(
    string UserId,
    string Email,
    string FirstName,
    string? MiddleName,
    string LastName,
    string? Phone,
    string? UserType,
    string BusinessType,
    string BusinessRegistrationNumber,
    string CompanyName,
    ShipperRoleType ShipperRole,
    BusinessProfileDto? BusinessProfile
);

public record CreateShipperDto
(
    string UserId,
    string Email,
    string FirstName,
    string? MiddleName,
    string LastName,
    string? Phone,
    string UserType,
    string BusinessType,
    string BusinessRegistrationNumber,
    string CompanyName,
    ShipperRoleType ShipperRole, 
    BusinessProfileDto? BusinessProfile
);

public record UpdateShipperDto
(
    string UserId,
    string Email,
    string? FirstName,
    string? MiddleName,
    string? LastName,
    string? Phone,
    string? UserType,
    string? BusinessType,
    string? BusinessRegistrationNumber,
    string? CompanyName,
    ShipperRoleType? ShipperRole,
    BusinessProfileDto? BusinessProfile
);
