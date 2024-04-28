namespace Frieght.Api.Dtos;

public record CarrierDto
(
    string UserId, // Unique identifier
    string Email,
    string FirstName,
    string LastName,
    string? Phone,
    string? MotorCarrierNumber,
    string? DOTNumber,
    string? EquipmentType,
    double? AvailableCapacity,
    string? CompanyName
);

public record CreateCarrierDto
(
    string UserId, // Unique identifier
    string Email,
    string FirstName,
    string LastName,
    string? Phone,
    string? MotorCarrierNumber,
    string? DOTNumber,
    string? EquipmentType,
    double? AvailableCapacity,
    string? CompanyName
);

public record UpdateCarrierDto
(
    string UserId, // Unique identifier
    string Email,
    string FirstName,
    string LastName,
    string? Phone,
    string? MotorCarrierNumber,
    string? DOTNumber,
    string? EquipmentType,
    double? AvailableCapacity,
    string? CompanyName
);
