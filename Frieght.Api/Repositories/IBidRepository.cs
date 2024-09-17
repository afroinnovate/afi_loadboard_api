using Frieght.Api.Entities;

namespace Frieght.Api.Repositories
{
    public interface IBidRepository
    {
        Task CreateBid(Bid bid);
        Task DeleteBid(int id);
        Task<Bid?> GetBid(int id);
        Task<IEnumerable<Bid>> GetBidsByCarrier(string carrierId);
        Task<Bid?> GetBidByLoadIdAndCarrierId(int loadId, string carrierId);
        Task<IEnumerable<Bid>> GetBids();
        Task UpdateBid(Bid bid);
    }
}