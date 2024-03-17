using System.ComponentModel.DataAnnotations;
using Frieght.Api.Entities;

namespace Frieght.Api.Dtos;

public record LoadDto
(
    int Id,
    string UserId,
    // string ShipperUserId,
    ShipperDto CreatedBy,
    string Origin,
    string destination ,
    DateTime PickupDate,
    DateTime DeliveryDate,
    string Commodity,
    double Weight ,
    decimal OfferAmount,
    string LoadDetails,
    string LoadStatus,
    int? AcceptedBidId,
    DateTime? Created,
    DateTime? Modified,
    string? ModifiedBy
);

public record CreateLoadDto
(
    string UserId,
    string ShipperUserId,
    [Required][StringLength(100)] string Origin,
    [Required][StringLength(100)] string destination,
    DateTime PickupDate,
    DateTime DeliveryDate,
    [Required][StringLength(30)] string Commodity,
    double Weight,
    decimal OfferAmount,
    [Required][StringLength(500)] string LoadDetails,
    [Required][StringLength(20)] string LoadStatus,
    DateTime Created
);

public record UpdateLoadDto
(
    string UserId,
    [Required][StringLength(100)] string Origin,
    [Required][StringLength(100)] string destination,
    DateTime PickupDate,
    DateTime DeliveryDate,
    [Required][StringLength(30)] string Commodity,
    double Weight,
    decimal OfferAmount,
    [Required][StringLength(500)] string LoadDetails,
    [Required][StringLength(20)] string LoadStatus,
    DateTime Modified,
    int? AcceptedBidId,
    string ModifiedBy
);



// public class LoadDto {
//     public int Id { get; set; }
//     public string UserId { get; set; }
//     public string ShipperUserId { get; set; }
//     public dynamic Shipper { get; set; }
//     public string Origin { get; set; }
//     public string destination { get; set; }
//     public DateTime PickupDate { get; set; }
//     public DateTime DeliveryDate { get; set; }
//     public string Commodity { get; set; }
//     public double Weight { get; set; }
//     public decimal OfferAmount { get; set; }
//     public string LoadDetails { get; set; }
//     public string LoadStatus { get; set; }
//     public int? AcceptedBidId { get; set; }
//     public DateTime? Created { get; set; }
//     public DateTime? Modified { get; set; }
//     public string ModifiedBy { get; set; }
// }