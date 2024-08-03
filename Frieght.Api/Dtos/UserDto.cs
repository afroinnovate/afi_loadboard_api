using Frieght.Api.Enuns;

namespace Frieght.Api.Dtos;

public record UserDto
(
    string UserId,
    string Email,
    string FirstName,
    string LastName,
    string? MiddleName,
    string? Phone,
    string UserType, // "Carrier" or "Shipper"
    BusinessProfileDto? BusinessProfile
);

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
    string? CompanyName,
    // Carrier specific
    string? MotorCarrierNumber,
    string? DOTNumber,
    string? EquipmentType,
    double? AvailableCapacity,
    string? BusinessType, // For shippers independent business owner, corporation, etc.
    string? BusinessRegistrationNumber,
    CarrierRoleType? CarrierRole,
    // Shipper specific
    ShipperRoleType? ShipperRole,
    // Vehicle information
    IEnumerable<VehicleTypeDto>? VehicleTypes
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
    ShipperRoleType? ShipperRole
);