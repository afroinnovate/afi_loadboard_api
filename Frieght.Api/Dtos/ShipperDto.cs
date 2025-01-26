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

public record ShipperDtoResponse
(
    string UserId,
    string Email,
    string FirstName,
    string? MiddleName,
    string LastName,
    string? Phone,
    string? UserType,
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
    string? idCardOrDriverLicenceNumber,
    ShipperRoleType ShipperRole
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
