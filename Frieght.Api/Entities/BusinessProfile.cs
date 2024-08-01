using Frieght.Api.Enuns;

namespace Frieght.Api.Entities
{
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
    public string? BusinessRegistrationNumber { get; set; }
    public string? IDCardOrDriverLicenceNumber { get; set; }
    public string? InsuranceName { get; set; }
    public string? BusinessType { get; set; } // For shippers(small business, large business, corporation, government)
    public CarrierRoleType? CarrierRole { get; set; } // For carriers(owner-operator, fleet owner)
    public ShipperRoleType? ShipperRole { get; set; } // For shippers(manufacturer, distributor, retailer, wholesaler)
    public virtual ICollection<BusinessVehicleType> BusinessVehicleTypes { get; set; } = new List<BusinessVehicleType>();
  }
}
