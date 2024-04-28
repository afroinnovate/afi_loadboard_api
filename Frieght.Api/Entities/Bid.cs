using Frieght.Api.Enuns;

namespace Frieght.Api.Entities;

public class Bid
{
    public int Id { get; set; }
    public int LoadId { get; set; }
    public string CarrierId { get; set; }
    public decimal BidAmount { get; set; }
    public BidStatus BidStatus { get; set; }
    public DateTimeOffset BiddingTime { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public string? UserType { get; set; }

    // Navigation properties
    public Load Load { get; set; }
    public User Carrier { get; set; }
}
