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
    /// <returns>None</returns>
    public async Task CreateBid(Bid bid)
    {
        try
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            _logger.LogInformation("Attempting to create bid for LoadId: {LoadId}", bid.LoadId);

            // Ensure the carrier is tracked and attach if necessary
            var trackedCarrier = await context.Users
                .Include(u => u.BusinessProfile)
                .ThenInclude(bp => bp.CarrierVehicles)
                .FirstOrDefaultAsync(u => u.UserId == bid.CarrierId);

            if (trackedCarrier == null)
            {
                _logger.LogWarning("Carrier not found, creating a new one.");

                if (bid.Carrier.BusinessProfile != null && bid.Carrier.BusinessProfile.CarrierVehicles != null)
                {
                    foreach (var vehicle in bid.Carrier.BusinessProfile.CarrierVehicles)
                    {
                        // Check if the VehicleType exists or create a new one
                        var vehicleType = await context.VehicleTypes.FirstOrDefaultAsync(vt => vt.Name == vehicle.Name);
                        if (vehicleType == null)
                        {
                            _logger.LogInformation("VehicleType '{VehicleName}' not found. Creating a new VehicleType.", vehicle.Name);
                            vehicleType = new VehicleType
                            {
                                Name = vehicle.Name
                            };
                            context.VehicleTypes.Add(vehicleType);
                            await context.SaveChangesAsync();  // Save to generate the VehicleTypeId
                        }

                        // Assign the VehicleTypeId to the vehicle
                        vehicle.VehicleTypeId = vehicleType.Id;
                        _logger.LogInformation($"VehicleType '{vehicle.Name}' created with Id: {vehicleType.Id}");

                        context.CarrierVehicle.Add(vehicle);
                    }
                }
                _logger.LogInformation("New Carrier Vehicle created with UserId: {UserId}", bid.Carrier.UserId);

                context.Users.Add(bid.Carrier);
            }
            else
            {
                bid.Carrier = trackedCarrier;
                _logger.LogInformation("Using existing carrier for the new bid.");
            }

            bid.Load = null;

            _logger.LogInformation("Committing the Bid to the database.");
            context.Bids.Add(bid);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Bid created successfully with BidId: {BidId}", bid.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating bid");
            await context.Database.RollbackTransactionAsync();
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
                                    .ThenInclude(l => l.Shipper)
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
                                    .ThenInclude(l => l.Shipper)
                                .Include(b => b.Carrier)
                                .Where(b => b.Load.LoadId == loadId && b.Carrier.UserId == carrierId)
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
                                    .ThenInclude(l => l.Shipper)
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

    #region GetBidsByLoadId
    /// <summary>
    /// Get all the bids by loadId
    /// </summary>
    /// <param name="loadId"></param>
    /// <returns>IEnumerable<Bid?></returns>
    public async Task<IEnumerable<Bid?>> GetBidsByLoadId(int loadId)
    {
        try
        {
            _logger.LogInformation("Retrieving all Bids by LoadId: {LoadId}", loadId);
            var bids = await context.Bids
                .Where(b => b.LoadId == loadId)
                .Include(b => b.Carrier)
                .AsNoTracking()
                .ToListAsync();
            
            _logger.LogInformation("Retrieved {BidCount} bids by LoadId: {LoadId}", bids.Count, loadId);
            return bids;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving bids by loadId, error: {Error}", ex.Message);
            throw;
        }
    }
    #endregion

    #region GetBidsByCarrier
    /// <summary>
    /// Get all the bids by carrier
    /// </summary>
    /// <param name="carrierId"></param>
    /// <returns>IEnumerable<Bid></returns>
    public async Task<IEnumerable<Bid>> GetBidsByCarrier(string carrierId)
    {
        try
        {
            _logger.LogInformation("Retrieving all Bids by CarrierId: {CarrierId}", carrierId);
            return await context.Bids
                .Include(b => b.Load)
                    .ThenInclude(l => l.Shipper)
                .Include(b => b.Carrier)
                .Where(b => b.Carrier.UserId == carrierId)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving bids by carrier");
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
            await context.Database.BeginTransactionAsync();

            _logger.LogInformation("Updating Bid: {Bid}", bid);
            context.Bids.Update(bid);
            await context.SaveChangesAsync();
            await context.Database.CommitTransactionAsync();
            _logger.LogInformation("Bid updated successfully with BidId: {BidId}", bid.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating bid");
            throw;
        }
    }
    #endregion
}
