using System.ComponentModel.DataAnnotations;

namespace Frieght.Api.Entities;

public class Load
{
    public int LoadId { get; set; }
    [Required]
    public required string ShipperUserId { get; set; }
    [Required]
    [StringLength(100)]
    public required string Origin { get; set; }
    [Required]
    [StringLength(100)]
    public required string Destination { get; set; }
    public DateTime PickupDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    [Required]
    [StringLength(30)]
    public required string Commodity { get; set; }
    public double Weight { get; set; }
    public decimal OfferAmount { get; set; }
    [Required]
    [StringLength(500)]
    public required string LoadDetails { get; set; }
    [Required]
    [StringLength(20)]
    public required string LoadStatus { get; set;}
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }

    // Navigation property to Shipper
    public User Shipper { get; set; }
}
