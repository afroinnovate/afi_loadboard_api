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
        string? UpdatedBy
);

public record CreateBidDto
(
        int LoadId,
        string CarrierId,
        decimal BidAmount,
        BidStatus BidStatus,
        DateTimeOffset BiddingTime
);

public record UpdateBidDto
(
        int Id,
        int LoadId,
        string CarrierId,
        decimal BidAmount,
        BidStatus BidStatus,
        DateTimeOffset BiddingTime,
        DateTimeOffset UpdatedAt,
        string UpdatedBy
);
