namespace Frieght.Api.Dtos
{
  public record VehicleTypeDto
  (
      int Id,
      string Name,
      IEnumerable<VehicleDto>? Vehicles
  );
}
