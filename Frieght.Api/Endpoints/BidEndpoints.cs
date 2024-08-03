using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Extensions;
using Frieght.Api.Repositories;
using Microsoft.AspNetCore.Routing;

namespace Frieght.Api.Endpoints;

public static class BidEndpoints
{
    public class LoggerCategory
    {
        // This class is used to define the category for the logger
    }

    const string GetBidEndpointName = "GetBid";
    const string GetBidByLoadAndCarrierEndpointName = "GetBidByLoadAndCarrier";

    public static RouteGroupBuilder MapBidsEndpoints(this IEndpointRouteBuilder routes)
    {

        var groups = routes.MapGroup("/bids")
            .WithParameterValidation();

        #region GetBidEndpoints
        groups.MapGet("/", async (IBidRepository repository) =>
            (await repository.GetBids()).Select(bid => bid.AsBidDto()));
        #endregion

        #region GetBidByIdEndpoint
        groups.MapGet("/{id}", async (IBidRepository repository, int id, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Getting Bid by Id {Id}", id);
                var bid = await repository.GetBid(id);
                if (bid == null) logger.LogInformation("Bid not found with Id {Id}", id);
                return bid != null ? Results.Ok(bid.AsBidDto()) : Results.NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting bid by id {0}", id);
                return Results.Problem("An error occurred while getting bid by id", statusCode: 500);
            }
        }).WithName(GetBidEndpointName);
        #endregion

        #region GetBidByLoadAndCarrierEndpoint
        groups.MapGet("/{loadId}/{carrierId}", async (IBidRepository repository, int loadId, string carrierId, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Getting Bid by Load Id and carrier id {loadId} with carrierId {carrierId}", loadId, carrierId);
                var bid = await repository.GetBidByLoadIdAndCarrierId(loadId, carrierId);
                return bid != null ? Results.Ok(bid.AsBidDto()) : Results.NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting bid by load id {0} and carrierId {1}", loadId, carrierId);
                return Results.Problem("An error occurred while getting bid by id", statusCode: 500);
            }
        }).WithName(GetBidByLoadAndCarrierEndpointName);
        #endregion

        #region CreateBidEndpoint
        groups.MapPost("/", async (IBidRepository repository, IUserRepository userRepo, ILoadRepository loadRepo, CreateBidDto bidDto, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Creating Bid");

                // Check if the load exists
                var load = await loadRepo.GetLoad(bidDto.LoadId);
                if (load == null) return Results.NotFound("Load not found");

                // Check if the user (carrier) exists
                var trackedCarrier = await userRepo.GetUser(bidDto.CarrierId);
                if (trackedCarrier == null)
                {
                    logger.LogWarning("Carrier not found. Creating a new user");
                    // Create a new user if not found
                    trackedCarrier = new User
                    {
                        UserId = bidDto.CarrierId,
                        FirstName = bidDto.CreatedBy.FirstName,
                        MiddleName = bidDto.CreatedBy.MiddleName,
                        LastName = bidDto.CreatedBy.LastName,
                        Email = bidDto.CreatedBy.Email,
                        Phone = bidDto.CreatedBy.Phone,
                        UserType = "Carrier",
                        BusinessProfile = new BusinessProfile
                        {
                            UserId = bidDto.CarrierId,
                            CompanyName = bidDto.CreatedBy.CompanyName,
                            DOTNumber = bidDto.CreatedBy.DOTNumber,
                            MotorCarrierNumber = bidDto.CreatedBy.MotorCarrierNumber,
                            EquipmentType = bidDto.CreatedBy.EquipmentType,
                            AvailableCapacity = bidDto.CreatedBy.AvailableCapacity,
                            CarrierRole = bidDto.CreatedBy.CarrierRole
                        }
                    };
                }

                var bid = new Bid
                {
                    LoadId = load.LoadId,
                    CarrierId = bidDto.CarrierId,
                    BidAmount = bidDto.BidAmount,
                    BidStatus = bidDto.BidStatus,
                    Load = load,
                    BiddingTime = DateTimeOffset.UtcNow,  // Set server-side for consistency
                    UpdatedAt = DateTimeOffset.UtcNow,    // Initial set at creation
                    Carrier = trackedCarrier              // Assign the user correctly
                };
                
                // check if a load is already bid by the carrier
                logger.LogInformation("Checking if bid already exists for LoadId {LoadId} and CarrierId {CarrierId}", bidDto.LoadId, bidDto.CarrierId);
                var existingBid = await repository.GetBidByLoadIdAndCarrierId(bidDto.LoadId, bidDto.CarrierId);
                if (existingBid != null)
                {
                    logger.LogWarning("Bid already exists for LoadId {LoadId} and CarrierId {CarrierId}", bidDto.LoadId, bidDto.CarrierId);
                    return Results.Conflict("Bid already exists for LoadId and CarrierId");
                }

                await repository.CreateBid(bid);

                var response = new BidDto
                (
                    Id: bid.Id,
                    LoadId: load.LoadId,
                    CarrierId: bid.CarrierId,
                    BidAmount: bid.BidAmount,
                    BidStatus: bid.BidStatus,
                    BiddingTime: bid.BiddingTime,
                    UpdatedAt: bid.UpdatedAt,
                    UpdatedBy: bid.UpdatedBy,
                    Load: new LoadDto
                    (
                        LoadId: load.LoadId,
                        ShipperUserId: load.ShipperUserId,
                        CreatedBy: load.Shipper.AsShipperDto(),
                        Origin: load.Origin,
                        Destination: load.Destination,
                        PickupDate: load.PickupDate,
                        DeliveryDate: load.DeliveryDate,
                        Commodity: load.Commodity,
                        Weight: load.Weight,
                        OfferAmount: load.OfferAmount,
                        LoadDetails: load.LoadDetails,
                        LoadStatus: load.LoadStatus,
                        CreatedAt: load.CreatedAt,
                        ModifiedAt: load.ModifiedAt,
                        ModifiedBy: load.ModifiedBy
                    ),
                    Carrier: trackedCarrier.AsCarrierDto()
                );

                return Results.CreatedAtRoute(GetBidEndpointName, new { id = bid.Id }, response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating bid");
                return Results.Problem("An error occurred while creating bid", statusCode: 500);
            }
        });
        #endregion


        #region UpdateBidEndpoint
        groups.MapPut("/{id}", async (IBidRepository repository, int id, UpdateBidDto updatedBidDto, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Updating Bid by Id {Id}", id);
                var existingBid = await repository.GetBid(id);
                if (existingBid == null) return Results.NotFound();

                existingBid.LoadId = updatedBidDto.LoadId;
                existingBid.CarrierId = updatedBidDto.CarrierId;
                existingBid.BidAmount = updatedBidDto.BidAmount;
                existingBid.BidStatus = updatedBidDto.BidStatus;
                existingBid.UpdatedAt = DateTimeOffset.UtcNow;  // Set server-side
                existingBid.UpdatedBy = updatedBidDto.UpdatedBy;

                await repository.UpdateBid(existingBid);
                logger.LogInformation("Bid Updated: {Bid}", existingBid);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating bid by id {0}", id);
                return Results.Problem("An error occurred while updating bid", statusCode: 500);
            }
        });
        #endregion

        #region DeleteBidEndpoint
        groups.MapDelete("/{id}", async (IBidRepository repository, int id, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Deleting Bid by Id {Id}", id);
                var bid = await repository.GetBid(id);
                if (bid == null) return Results.NotFound();

                await repository.DeleteBid(id);
                logger.LogInformation("Bid Deleted: {Id}", id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting bid by id {0}", id);
                return Results.Problem("An error occurred while deleting bid", statusCode: 500);
            }
        });
        #endregion

        return groups;
    }
}
