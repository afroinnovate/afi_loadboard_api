using Frieght.Api.Entities;
using Frieght.Api.Enuns;

namespace Frieght.Api.Dtos;

public record BidDto
(
    int Id,
    int LoadId,
    string CarrierId,
    decimal BidAmount,
    BidStatus BidStatus,
    DateTimeOffset BiddingTime,
    DateTimeOffset UpdatedAt,
    string? UpdatedBy,
    LoadDto Load,    // Include associated Load details
    CarrierDto Carrier // Include associated Carrier details
);

public record BidDtoResponse
(
    int Id,
    int LoadId,
    string CarrierId,
    decimal BidAmount,
    BidStatus BidStatus,
    DateTimeOffset BiddingTime,
    DateTimeOffset UpdatedAt,
    string? UpdatedBy,
    LoadDtoResponse Load,    // Include associated Load details
    CarrierResponse Carrier // Include associated Carrier details
);

public record UpdateBidDto
(
    int Id,
    int LoadId,
    string CarrierId,
    decimal BidAmount,
    LoadDto Load,
    BidStatus BidStatus,
    DateTimeOffset BiddingTime, // Review if updates to BiddingTime are allowed
    DateTimeOffset UpdatedAt,   // This should likely be set automatically to now in the backend
    string UpdatedBy            // Ensure this is required if updates are made
);

public class CreateBidDto
{
    public int LoadId { get; set; }
    public required string CarrierId { get; set; }
    public decimal BidAmount { get; set; }
    public int BidStatus { get; set; }
    public DateTimeOffset BiddingTime { get; set; }
    public required CreateCarrierDto CreatedBy { get; set; }
}
