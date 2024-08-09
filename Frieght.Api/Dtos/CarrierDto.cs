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
    VehicleDto[] Vehicles,
    CarrierRoleType CarrierRole
);

public class CreateCarrierDto
{
    public required string UserId { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string MiddleName { get; set; }
    public required string LastName { get; set; }
    public required string Phone { get; set; }
    public required string UserType { get; set; }
    public required string DotNumber { get; set; }
    public required string MotorCarrierNumber { get; set; }
    public required string EquipmentType { get; set; }
    public double AvailableCapacity { get; set; }
    public required string CompanyName { get; set; }
    public required string Name { get; set; }  // Vehicle Name
    public required string Description { get; set; }
    public required string ImageUrl { get; set; }
    public required string Vin { get; set; }
    public required string LicensePlate { get; set; }
    public required string Make { get; set; }
    public required string Model { get; set; }
    public required string Year { get; set; }
    public required string Color { get; set; }
    public bool HasInsurance { get; set; }
    public bool HasRegistration { get; set; }
    public bool HasInspection { get; set; }
    public int Quantity { get; set; }
    public CarrierRoleType CarrierRole { get; set; }
}

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
    string VehicleTypeName,
    int Quantity,
    bool HasInsurance,
    bool HasRegistration,
    bool HasInspection,
    string? Description,
    string? ImageUrl,
    string VIN,
    string? LicensePlate,
    string? Make,
    string? Model,
    string? Year,
    string? Color,
    CarrierRoleType CarrierRole
);
