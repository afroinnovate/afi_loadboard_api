using Frieght.Api.Entities;
using Frieght.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using Frieght.Api.Dtos;

namespace Frieght.Api.Repositories
{
    public class LoadRepository : ILoadRepository
    {
        private readonly FrieghtDbContext context;
        ILogger<LoadRepository> _logger;
        public LoadRepository(FrieghtDbContext context, ILogger<LoadRepository> logger)
        {
            this.context = context;
            _logger = logger;
        }
        
        public async Task CreateLoad(Load load, User shipper)
        {
            _logger.LogInformation("Attempting to create load for ShipperUserId: {ShipperUserId}", load.ShipperUserId);

            try
            {
                // Start transaction to ensure atomicity
                using var transaction = context.Database.BeginTransaction();

                // Check if the Shipper already exists
                var trackedShipper = await context.Users
                            .FirstOrDefaultAsync(u => u.UserId == shipper.UserId);

                if (trackedShipper == null)
                {
                    _logger.LogInformation("Shipper not found, creating a new one.");
                    if (shipper == null)
                    {
                        _logger.LogError("Shipper information is missing in the request.");
                        throw new ArgumentNullException("Shipper", "Shipper information is required to create a new load.");
                    }

                    // Add the new shipper if not tracked
                    context.Users.Add(shipper);
                    load.Shipper = shipper;
                    _logger.LogInformation("New Shipper created with UserId: {UserId}", shipper.UserId);
                }
                else
                {
                    // Use the tracked instance for all operations
                    load.Shipper = trackedShipper;
                    _logger.LogInformation("Using existing shipper for the new load.");
                }

                // Add the load
                context.Loads.Add(load);
                await context.SaveChangesAsync();
                await transaction.CommitAsync(); // Commit the transaction

                _logger.LogInformation("Load created successfully with LoadId: {LoadId}", load.LoadId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create load due to an error.");
                throw; // Rethrow to ensure the error is handled or logged at a higher level
            }
        }


        public async Task DeleteLoad(int id)
        {
            _logger.LogInformation("Deleting Load: {Id}", id);
            var load = await context.Loads.FindAsync(id);
            if (load != null)
            {
                context.Loads.Remove(load);
                try
                {
                    await context.SaveChangesAsync();
                    _logger.LogInformation("Load Deleted");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting load");
                    throw;
                }
            }
            else
            {
                _logger.LogWarning("Load not found for deletion: {Id}", id);
            }
        }

        public async Task<Load?> GetLoad(int id)
        {
            _logger.LogInformation("Attempting to retrieve Load with ID: {Id}", id);
            try
            {
                var load = await context.Loads
                    .Include(l => l.Shipper) // Eager load the Shipper associated with the Load
                    .AsNoTracking()          // Use AsNoTracking for read-only operations for better performance
                    .FirstOrDefaultAsync(l => l.LoadId == id);

                if (load == null)
                {
                    _logger.LogWarning("No Load found with ID: {Id}", id);
                    return null;
                }

                _logger.LogInformation("Load retrieved successfully with ID: {Id}", id);
                return load;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Load with ID: {Id}", id);
                throw;  // Rethrow the exception to allow further handling up the stack if necessary
            }
        }
        public async Task<IEnumerable<Load>> GetLoads()
        {
            _logger.LogInformation("Attempting to retrieve all Loads");
            try
            {
                var loads = await context.Loads
                    .Include(l => l.Shipper)  // Eager load the Shipper associated with each Load
                    .AsNoTracking()           // Use AsNoTracking for better performance in read-only operations
                    .ToListAsync();

                if (loads == null || !loads.Any())
                {
                    _logger.LogInformation("No Loads found in the database.");
                    return new List<Load>();  // Return an empty list instead of null
                }

                _logger.LogInformation("Successfully retrieved {Count} Loads.", loads.Count);
                return loads;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all Loads");
                return new List<Load>();  // Return an empty list instead of null to maintain consistency
            }
        }

        public async Task UpdateLoad(Load load)
        {
            _logger.LogInformation("Updating Load with ID: {LoadId}", load.LoadId);
            try
            {
                context.Loads.Update(load);
                await context.SaveChangesAsync();
                _logger.LogInformation("Load with ID: {LoadId} updated successfully", load.LoadId);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error while updating Load with ID: {LoadId}", load.LoadId);
                throw; // Throw to let higher layers handle or log the specific error
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Load with ID: {LoadId}", load.LoadId);
                throw; // General error handling, rethrow to maintain stack trace and allow further handling
            }
        }
    }
}
