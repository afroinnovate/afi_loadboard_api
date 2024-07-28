using Frieght.Api.Enuns;

namespace Frieght.Api.Entities;

public class BusinessProfile
{
  public int Id { get; set; }
  public required string UserId { get; set; }
  public required virtual User User { get; set; }
  public string? MotorCarrierNumber { get; set; }
  public string? DOTNumber { get; set; }
  public string? EquipmentType { get; set; }
  public double? AvailableCapacity { get; set; }
  public string? CompanyName { get; set; }
  public CarrierRoleType? CarrierRole { get; set; }
  public ShipperRoleType? ShipperRole { get; set; }

  // Navigation property for the many-to-many relationship with VehicleTypes
  public virtual ICollection<BusinessVehicleType>? BusinessVehicleTypes { get; set; } = new List<BusinessVehicleType>();
}