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
        
        public async Task CreateLoad(CreateLoadDto loadDto)
        {
            _logger.LogInformation("Creating Load");

           // Check if the Shipper already exists
            var existingShipper = await context.Shipper.FindAsync(loadDto.ShipperUserId);
            _logger.LogInformation("Shipper found: {0}", existingShipper);
            // If the Shipper doesn't exist, create a new one
            if (existingShipper == null)
            {
                _logger.LogInformation("Shipper Doesn't exist, Creating new Shipper");
                if (loadDto.CreatedBy == null)
                {
                    _logger.LogError("Shipper information is missing in the request.");
                    throw new ArgumentNullException(nameof(loadDto.CreatedBy), "Shipper information is required to create a new Shipper.");
                }

                Shipper newShipper = new Shipper()
                {
                    UserId = loadDto.ShipperUserId,
                    Email = loadDto.CreatedBy.Email,
                    CompanyName = loadDto.CreatedBy.CompanyName,
                    DOTNumber = loadDto.CreatedBy.DOTNumber,
                    FirstName = loadDto.CreatedBy.FirstName,
                    LastName = loadDto.CreatedBy.LastName,
                    Loads = null
                    // Set other properties as needed, or leave them null
                };

                context.Shipper.Add(newShipper);
                _logger.LogInformation("Shipper Created");
            }

            _logger.LogInformation("Creating Load Record");
            Load load = new()
            {
                UserId = loadDto.UserId,
                ShipperUserId = loadDto.ShipperUserId,
                Origin = loadDto.Origin,
                Destination = loadDto.Destination,
                PickupDate = loadDto.PickupDate,
                DeliveryDate = loadDto.DeliveryDate,
                Commodity = loadDto.Commodity,
                Weight = loadDto.Weight,
                OfferAmount = loadDto.OfferAmount,
                LoadDetails = loadDto.LoadDetails,
                LoadStatus = loadDto.LoadStatus,
                Created = DateTime.UtcNow
            };
            try
            {
                await context.Loads.AddAsync(load);
                await context.SaveChangesAsync();
                _logger.LogInformation("Load Created");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating Load: {0}", ex.Message);
            }
        }

        public async Task DeleteLoad(int id)
        {
            _logger.LogInformation("Deleting Load");
            var load = context.Loads.Find(id);
            try
            {
                if (load is not null)
                {
                    context.Loads.Remove(load);
                    await context.SaveChangesAsync();
                    _logger.LogInformation("Load Deleted");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting Load: {0}", ex.Message);
            }
        }

        public async Task<Load?> GetLoad(int id)
        {
            _logger.LogInformation("Getting Load");
            try
            {
                var load = await context.Loads.Include(s => s.Shipper).FirstOrDefaultAsync(l => l.Id == id);
                _logger.LogInformation("Load Found: {0}", load);
                return load;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting Load: {0}", ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<Load>> GetLoads()
        {
            _logger.LogInformation("Getting Loads");
            try
            {
                var loads = await context.Loads.Include(s => s.Shipper).AsNoTracking().ToListAsync();
                _logger.LogInformation("Loads Found: {0}", loads);
                return loads;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting Loads: {0}", ex.Message);
                return null;
            }
        }

        public async Task UpdateLoad(Load load)
        {
            _logger.LogInformation("Updating Load");
            try
            {
                context.Loads.Update(load);
                await context.SaveChangesAsync();
                _logger.LogInformation("Load Updated");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating Load: {0}", ex.Message);
            }
        }
    }
}
