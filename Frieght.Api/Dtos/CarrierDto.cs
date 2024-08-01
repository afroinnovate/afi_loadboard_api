using Frieght.Api.Enuns;
namespace Frieght.Api.Dtos;

public record CarrierDto
(
    string UserId,
    string Email,
    string FirstName,
    string? MiddleName,
    string LastName,
    string? Phone,
    string? UserType,
    string? DOTNumber,
    string? MotorCarrierNumber,
    string? EquipmentType,
    double? AvailableCapacity,
    string? CompanyName,
    VehicleTypeDto[]? VehicleTypes,
    CarrierRoleType? CarrierRole,
    BusinessProfileDto? BusinessProfile
);

public record CreateCarrierDto
(
    string UserId,
    string Email,
    string FirstName,
    string? MiddleName,
    string LastName,
    string? Phone,
    string UserType,
    string? DOTNumber,
    string? MotorCarrierNumber,
    string? EquipmentType,
    double? AvailableCapacity,
    string? CompanyName,
    CarrierRoleType? CarrierRole,
    VehicleTypeDto[]? VehicleTypes,
    BusinessProfileDto? BusinessProfile
);

public record UpdateCarrierDto
(
    string UserId,
    string Email,
    string FirstName,
    string? MiddleName,
    string LastName,
    string? Phone,
    string? UserType,
    string? DOTNumber,
    string? MotorCarrierNumber,
    string? EquipmentType,
    double? AvailableCapacity,
    string? CompanyName,
    CarrierRoleType? CarrierRole,
    VehicleTypeDto[]? VehicleTypes,
    BusinessProfileDto? BusinessProfile
);
