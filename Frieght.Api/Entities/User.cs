using Frieght.Api.Enuns;

namespace Frieght.Api.Entities;
public class User
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string? MiddleName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Phone { get; set; }
    public string? MotorCarrierNumber { get; set; } // Primarily for carriers
    public string? DOTNumber { get; set; } // Primarily for carriers
    public string? EquipmentType { get; set; } // Primarily for carriers
    public double? AvailableCapacity { get; set; } // Primarily for carriers
    public string? CompanyName { get; set; } // Could be used by both, but mainly carriers
    public string? UserType { get; set; } // Shipper or Carrier
    public CarrierRoleType? CarrierRole { get; set; } // Owner Operator, Fleet Owner, or Company Driver
    public ShipperRoleType? ShipperRole { get; set; } // Manufacturer, Distributor, or Retailer
    // Navigation properties
    public virtual ICollection<Load>? Loads { get; set; } = new List<Load>(); // List of loads posted by the user
    public virtual ICollection<Bid>? Bids { get; set; } = new List<Bid>(); // List of bids placed by the user
}
