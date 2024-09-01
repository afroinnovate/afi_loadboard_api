namespace Frieght.Api.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int VehicleTypeId { get; set; }
        public int BusinessProfileId { get; set; }  // Foreign key to BusinessProfile
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? VIN { get; set; }
        public string? LicensePlate { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? Year { get; set; }
        public string? Color { get; set; }
        public bool HasInsurance { get; set; }
        public bool HasRegistration { get; set; }
        public bool HasInspection { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        // Navigation properties
        public virtual BusinessProfile? BusinessProfile { get; set; }
        public virtual VehicleType? VehicleType { get; set; }
    }
}