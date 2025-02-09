using Frieght.Api.Enuns;

namespace Frieght.Api.Dtos;

public class UserDto
{
    public required string UserId { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? MiddleName { get; init; }
    public string? Phone { get; init; }
    public required string UserType { get; init; }
    public BusinessProfileDto? BusinessProfile { get; init; }
    public string? FullName { get; set; }
    public string? Company { get; set; }
}

public record CreateUserDto
(
    string UserId,
    string Email,
    string FirstName,
    string? MiddleName,
    string LastName,
    string? Phone,
    string UserType, // "Carrier" or "Shipper"
                    // Common fields for both
    // Vehicle information
    BusinessProfileDto? BusinessProfile
);

public record UpdateUserDto
(
    string Email,
    string FirstName,
    string LastName,
    string? Phone,
    string UserType, // "Carrier" or "Shipper"
    string? CompanyName,
    string? MotorCarrierNumber,
    string? DOTNumber,
    string? EquipmentType,
    double? AvailableCapacity,
    CarrierRoleType? CarrierRole,
    ShipperRoleType? ShipperRole,
    string? BusinessType, // For shippers independent business owner, corporation, etc.
    string? BusinessRegistrationNumber,
    BusinessProfileDto? BusinessProfile
);