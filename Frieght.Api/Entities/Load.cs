using System.ComponentModel.DataAnnotations;

namespace Frieght.Api.Entities;

public class Load
{
    public int Id { get; set; }
    [Required]
    public required string UserId { get; set; }
    [Required]
    [StringLength(100)]
    public required string Origin { get; set; }
    [Required]
    [StringLength(100)]
    public required string destination { get; set; }
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
    public int? AcceptedBidId { get; set; }

    // Navigation property for EF Core to establish the foreign key relationship
    // public Bid? AcceptedBid { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
    public string? ModifiedBy { get; set; }
}
