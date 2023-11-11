using System.ComponentModel.DataAnnotations;

namespace Frieght.Api.Dtos;

public record LoadDto
(
    int Id,
    string UserId,
    string Origin,
    string destination ,
    DateTime PickupDate,
    DateTime DeliveryDate,
    string Commodity,
    double Weight ,
    decimal OfferAmount,
    string LoadDetails,
    string LoadStatus
);

public record CreateLoadDto
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
    [Required][StringLength(20)] string LoadStatus
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
    [Required][StringLength(20)] string LoadStatus
);
