﻿using Frieght.Api.Entities;

namespace Frieght.Api.Repositories
{
    public interface IBidRepository
    {
        Task CreateBid(Bid bid, User carrier);
        Task DeleteBid(int id);
        Task<Bid?> GetBid(int id);
        Task<Bid?> GetBidByLoadId(int id);
        Task<IEnumerable<Bid>> GetBids();
        Task UpdateBid(Bid bid);
    }
}