using Frieght.Api.Entities;
using Frieght.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Frieght.Api.Repositories;

public class BidRepository : IBidRepository
{

    ILogger<LoadRepository> _logger;
    private readonly FrieghtDbContext context;

    public BidRepository(FrieghtDbContext context, ILogger<LoadRepository> logger)
    {
        this.context = context;
        _logger = logger;
    }
    
    public async Task CreateBid(Bid bid)
    {
        try
        {
            _logger.LogInformation("Creating Bid: {0}", bid);
            context.Add(bid);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating bid");
            throw ex;
        }
    }

    public async Task DeleteBid(int id)
    {
        try
        {
            _logger.LogInformation("Deleting Bid: {0}", id);
            var bid = await context.Bids.FindAsync(id);
            if (bid is not null) context.Remove(bid);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting bid");
            throw ex;
        }
    }

    public async Task<Bid?> GetBid(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving Bid: {0}", id);
            return await context.Bids.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving bid");
            throw ex;
        }
    }

    public async Task<Bid?> GetBidByLoadId(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving Bid by loadId: {0}", id);
            return await context.Bids.FirstOrDefaultAsync(b => b.LoadId == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving bid");
            throw ex;
        }
    }

    public async Task<IEnumerable<Bid>> GetBids()
    {
        try
        {
            _logger.LogInformation("Retrieving Bids");
            return await context.Bids.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving bids");
            throw ex;
        }
    }

    public async Task UpdateBid(Bid bid)
    {
        try
        {
            _logger.LogInformation("Updating Bid: {0}", bid);
            context.Update(bid);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating bid");
            throw ex;
        }
    }
}


