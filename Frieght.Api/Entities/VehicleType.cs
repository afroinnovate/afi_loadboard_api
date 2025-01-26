namespace Frieght.Api.Entities
{
  public class VehicleType
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public virtual ICollection<Vehicle>? Vehicles { get; set; }
  }
}
