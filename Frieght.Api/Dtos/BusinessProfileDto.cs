using Frieght.Api.Enuns;

namespace Frieght.Api.Dtos
{
  public record BusinessProfileDto
  (
    string UserId,
    string CompanyName,
    string? MotorCarrierNumber,
    string? DOTNumber,
    string? EquipmentType,
    double? AvailableCapacity,
    string? IDCardOrDriverLicenceNumber,
    string? InsuranceName,
    string? BusinessType,
    CarrierRoleType? CarrierRole,
    ShipperRoleType? ShipperRole,
    string? BusinessRegistrationNumber,
    List<VehicleTypeDto>? VehicleTypes
  );
}
