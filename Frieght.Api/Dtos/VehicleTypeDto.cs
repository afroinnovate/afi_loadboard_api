namespace Frieght.Api.Dtos;
public class VehicleTypeDto
{
  public required string Name { get; set; }
  public int Quantity { get; set; }
  public bool? HasInsurance { get; set; }
  public bool? HasRegistration { get; set; }
  public bool? HasInspection { get; set; }
  public string? Description { get; set; }
  public string? ImageUrl { get; set; }
  public string? VIN { get; set; }
  public string? LicensePlate { get; set; }
  public string? Make { get; set; }
  public string? Model { get; set; }
  public string? Year { get; set; }
  public string? Color { get; set; }
}