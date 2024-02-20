using Frieght.Api.Entities;
using Frieght.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Frieght.Api.Repositories;

public class BidRepository : IBidRepository
{
    private readonly FrieghtDbContext context;

    public BidRepository(FrieghtDbContext context)
    {
        this.context = context;
    }
    public async Task CreateBid(Bid bid)
    {
        context.Add(bid);
        await context.SaveChangesAsync();
    }

    public async Task DeleteBid(int id)
    {
        var bid = context.Bids.Find(id);
        if (bid is not null) context.Remove(bid);
        await context.SaveChangesAsync();
    }

    public async Task<Bid?> GetBid(int id)
    {
        return await context.Bids.FindAsync(id);
    }

    public async Task<IEnumerable<Bid>> GetBids()
    {
        return await context.Bids.AsNoTracking().ToListAsync();
    }

    public async Task UpdateBid(Bid bid)
    {
        context.Update(bid);
        await context.SaveChangesAsync();
    }
}


