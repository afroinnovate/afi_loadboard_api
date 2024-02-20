using Frieght.Api.Enuns;

namespace Frieght.Api.Dtos;

public record BidDto
(
        int Id,
        int LoadId,
        int CarrierId,
        decimal BidAmount,
        BidStatus BidStatus,
        DateTimeOffset BiddingTime

);

public record CreateBidDto
(

        int LoadId,
        int CarrierId,
        decimal BidAmount,
        BidStatus BidStatus,
        DateTimeOffset BiddingTime

);

public record UpdateBidDto
(
        int Id,
        int LoadId,
        int CarrierId,
        decimal BidAmount,
        BidStatus BidStatus,
        DateTimeOffset BiddingTime

);
