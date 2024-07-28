using Frieght.Api.Entities;

public class BusinessVehicleType
{
  public int BusinessProfileId { get; set; }
  public required virtual BusinessProfile BusinessProfile { get; set; }
  public int VehicleTypeId { get; set; }
  public required virtual VehicleType VehicleType { get; set; }
  public int Quantity { get; set; }
}