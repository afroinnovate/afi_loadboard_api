namespace Frieght.Api.Entities;

public class Carrier
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string CompanyName { get; set; }
    public string CompanyPhone { get; set;}
    public string CompanyEmail { get; set;}
    public string MotorCarrierNumber { get; set;}
    public string USDOTNumber { get; set; }
    public string EquipmentType { get; set; }
    public double AvailableCapacity { get; set; }


}
