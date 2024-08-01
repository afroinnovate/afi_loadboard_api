using Frieght.Api.Enuns;

namespace Frieght.Api.Dtos
{
  public class BusinessProfileDto
  {
    public required string UserId { get; set; }
    public required string CompanyName { get; set; }
    public string? MotorCarrierNumber { get; set; }
    public string? DOTNumber { get; set; }
    public string? EquipmentType { get; set; }
    public double? AvailableCapacity { get; set; }
    public string? IDCardOrDriverLicenceNumber { get; set; }
    public string? InsuranceName { get; set; }
    public string? BusinessType { get; set; } // For shippers
    public CarrierRoleType? CarrierRole { get; set; } // For carriers
    public ShipperRoleType? ShipperRole { get; set; } // For shippers
    public string? BusinessRegistrationNumber { get; set; }
    public List<VehicleTypeDto>? VehicleTypes { get; set; }
  }
}
