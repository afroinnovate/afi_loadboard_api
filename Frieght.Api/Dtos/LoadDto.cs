using System.ComponentModel.DataAnnotations;
using Frieght.Api.Entities;

namespace Frieght.Api.Dtos;

public record LoadDto
(
    int LoadId,
    string ShipperUserId,
    ShipperDto CreatedBy,
    string Origin,
    string Destination ,
    DateTime PickupDate,
    DateTime DeliveryDate,
    string Commodity,
    double Weight ,
    decimal OfferAmount,
    string LoadDetails,
    string LoadStatus,
    DateTime? CreatedAt,
    DateTime? ModifiedAt,
    string? ModifiedBy
);

public record LoadDtoResponse
(
    int LoadId = 0,
    string ShipperUserId = "",
    ShipperDtoResponse? CreatedBy = null,
    string Origin = "",
    string Destination = "",
    DateTime PickupDate = default,
    DateTime DeliveryDate = default,
    string Commodity = "",
    double Weight = 0,
    decimal OfferAmount = 0,
    string LoadDetails = "",
    string LoadStatus = "",
    double EstimatedDistance = 0,
    DateTime? CreatedAt = null,
    DateTime? ModifiedAt = null,
    string? ModifiedBy = null
);

public record CreateLoadDto
(
    string ShipperUserId, // User creating the load
    [Required][StringLength(100)] string Origin,
    [Required][StringLength(100)] string Destination,
    DateTime PickupDate,
    DateTime DeliveryDate,
    [Required][StringLength(30)] string Commodity,
    double Weight,
    decimal OfferAmount,
    [Required][StringLength(500)] string LoadDetails,
    [Required][StringLength(20)] string LoadStatus,
    double EstimatedDistance,
    DateTime CreatedAt,
    CreateShipperDto CreatedBy
);

public record UpdateLoadDto
(
    string ShipperUserId, // Consider if this is necessary on update or if other fields are needed
    [Required][StringLength(100)] string Origin,
    [Required][StringLength(100)] string Destination,
    DateTime PickupDate,
    DateTime DeliveryDate,
    [Required][StringLength(30)] string Commodity,
    double Weight,
    decimal OfferAmount,
    [Required][StringLength(500)] string LoadDetails,
    [Required][StringLength(20)] string LoadStatus,
    double EstimatedDistance,
    DateTime ModifiedAt,
    string ModifiedBy
);