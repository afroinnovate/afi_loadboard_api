namespace Frieght.Api.Dtos
{
    public record VehicleDto
    (
        int Id,
        int VehicleTypeId,
        string Name,
        string Description,
        string ImageUrl,
        string VIN,
        string LicensePlate,
        string Make,
        string Model,
        string Year,
        string Color,
        bool HasInsurance,
        bool HasRegistration,
        bool HasInspection,
        DateTimeOffset? CreatedAt,
        DateTimeOffset? UpdatedAt,
        string? CreatedBy,
        string? UpdatedBy,
        string? TruckLength,
        string? TruckHeight
    );
}