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
    VehicleTypeDto[] VehicleTypes,
    CarrierRoleType CarrierRole
);

public record CarrierResponse
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
    VehicleTypeDto[] VehicleTypes,
    CarrierRoleType CarrierRole
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
    string Name,
    int Quantity,
    bool? HasInsurance,
    bool? HasRegistration,
    bool? HasInspection,
    string? Description,
    string? ImageUrl,
    string? VIN,
    string? LicensePlate,
    string? Make,
    string? Model,
    string? Year,
    string? Color,
    CarrierRoleType CarrierRole
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
    string Name,
    int Quantity,
    bool? HasInsurance,
    bool? HasRegistration,
    bool? HasInspection,
    string? Description,
    string? ImageUrl,
    string? VIN,
    string? LicensePlate,
    string? Make,
    string? Model,
    string? Year,
    string? Color,
    CarrierRoleType CarrierRole
);
