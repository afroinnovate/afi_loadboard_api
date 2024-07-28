using Frieght.Api.Enuns;

namespace Frieght.Api.Dtos;
public class BusinessProfileDto
{
  public required string UserId { get; set; }
  public required string CompanyName { get; set; }
  public CarrierRoleType Role { get; set; }
  public string? MotorCarrierNumber { get; set; }
  public string? DOTNumber { get; set; }
  public string? EquipmentType { get; set; }
  public double? AvailableCapacity { get; set; }
  public List<VehicleTypeDto>? VehicleTypes { get; set; }
}