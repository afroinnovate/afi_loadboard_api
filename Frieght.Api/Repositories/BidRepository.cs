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
    
    #region CreateBid
    /// <summary>
    /// Create a bid
    /// </summary>
    /// <param name="bid"></param>
    /// <param name="carrier"></param>
    /// <returns>None</returns>
    public async Task CreateBid(Bid bid, User carrier)
    {
        try
        {
            using var transaction = context.Database.BeginTransaction();

            _logger.LogInformation("Attempting to create bid for LoadId: {LoadId}", bid.LoadId);

            // Check if the Shipper already exists
            var existingCarrier = await context.Users
                            .FirstOrDefaultAsync(u => u.UserId == bid.CarrierId);
                            
            if (existingCarrier == null)
            {
                _logger.LogInformation("Carrier not found, creating a new one.");
                if (carrier == null)
                {
                    _logger.LogError("Carrier information is missing in the request.");
                    throw new ArgumentNullException("Carrier", "Carrier information is required to create a new bid.");
                }
                context.Users.Add(carrier);
                _logger.LogInformation("New Carrier created with UserId: {UserId}", carrier.UserId);
            }
            context.Bids.Add(bid);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            _logger.LogInformation("Bid created successfully with BidId: {BidId}", bid.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating bid");
            throw;
        }
    }
    #endregion

    #region DeleteBid
    /// <summary>
    /// Delete a bid
    /// </summary>
    /// <param name="id"></param>
    /// <returns>None</returns>
    public async Task DeleteBid(int id)
    {
        try
        {
            _logger.LogInformation("Deleting Bid: {Id}", id);
            var bid = await context.Bids.FindAsync(id);
            if (bid != null) 
            {
                context.Bids.Remove(bid);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting bid");
            throw;
        }
    }
    #endregion

    #region GetBid
    /// <summary>
    /// Get Bid by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Bid Object</returns>
    public async Task<Bid?> GetBid(int id)
    {
       try
        {
            _logger.LogInformation("Retrieving Bid: {Id}", id);
            return await context.Bids
                                .Include(b => b.Load)
                                .Include(b => b.Carrier)
                                .FirstOrDefaultAsync(b => b.Id == id);                
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving bid");
            throw;
        }
    }
    #endregion

    #region GetBidByLoadIdAndCarrierId
    /// <summary>
    /// Get Bid by LoadId and CarrierId
    /// </summary>
    /// <param name="loadId"></param>
    /// <param name="carrierId"></param>
    /// <returns>Bid Object</returns>
    public async Task<Bid?> GetBidByLoadIdAndCarrierId(int loadId, string carrierId)
    {
        try
        {
            _logger.LogInformation("Retrieving Bid by LoadId: {LoadId} and carrier id: {CarrierId}", loadId, carrierId);
            return await context.Bids
                                .Include(b => b.Load)
                                .Include(b => b.Carrier)
                                .Where(b => b.LoadId == loadId && b.CarrierId == carrierId)
                                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving bid by loadId");
            throw;
        }
    }
    #endregion

    #region GetBids
    /// <summary>
    /// Get all the bids
    /// </summary>
    /// <returns>IEnumerable<Bid></returns>
    public async Task<IEnumerable<Bid>> GetBids()
    {
        try
        {
            _logger.LogInformation("Retrieving all Bids");
            return await context.Bids
                                .Include(b => b.Load)
                                .Include(b => b.Carrier)
                                .AsNoTracking()
                                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all bids");
            throw;
        }
    }
    #endregion

    #region UpdateBid
    /// <summary>
    /// Update a bid
    /// </summary>
    /// <param name="bid"></param>
    /// <returns>None</returns>
    public async Task UpdateBid(Bid bid)
    {
        try
        {
            _logger.LogInformation("Updating Bid: {Bid}", bid);
            context.Bids.Update(bid);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating bid");
            throw;
        }
    }
    #endregion
}
