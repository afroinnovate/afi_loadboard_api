namespace Frieght.Api.Dtos
{
  public record VehicleTypeDto
  (
      string Name,
      string? Description,
      string? ImageUrl,
      string? VIN,
      string? LicensePlate,
      string? Make,
      string? Model,
      string? Year,
      string? Color,
      bool? HasInsurance,
      bool? HasRegistration,
      bool? HasInspection,
      int Quantity
  );
}
