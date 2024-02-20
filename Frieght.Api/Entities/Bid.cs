using Frieght.Api.Enuns;

namespace Frieght.Api.Entities;

public class Bid
{
    public int Id { get; set; }
    public int LoadId { get; set; }
    public int CarrierId { get; set; }
    public decimal BidAmount { get; set; }
    public BidStatus BidStatus { get; set; }
    public DateTimeOffset BiddingTime { get; set; }
}
