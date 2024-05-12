using Frieght.Api.Dtos;
using Frieght.Api.Entities;
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
            (await repository.GetBids()).Select(bid => bid.asDto()));
        #endregion

        #region GetBidByIdEndpoint
        groups.MapGet("/{id}", async (IBidRepository repository, int id,  ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Getting Bid by Id {Id}", id);
                var bid = await repository.GetBid(id);
                return bid != null ? Results.Ok(bid.asDto()) : Results.NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting bid by id {0}", id);
                return Results.Problem("An error occurred while getting bid by id", statusCode: 500);
            }
        }).WithName(GetBidEndpointName);
        #endregion

        #region GetBidByLoadAndCarrierEndpoint
        groups.MapGet("/{loadId}/{carrierId}", async (IBidRepository repository, int loadId, string carrierId,  ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Getting Bid by Load Id and carrier id {loadId} with carrierId {carrierId}", loadId, carrierId);
                var bid = await repository.GetBidByLoadIdAndCarrierId(loadId, carrierId);
                return bid != null ? Results.Ok(bid.asDto()) : Results.NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting bid by load id {0} and carrierId {1}", loadId, carrierId);
                return Results.Problem("An error occurred while getting bid by id", statusCode: 500);
            }
        }).WithName(GetBidByLoadAndCarrierEndpointName);
        #endregion

        #region CreateBidEndpoint
        groups.MapPost("/", async (IBidRepository repository, ICarrierRepository carrierRepo, CreateBidDto bidDto, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Creating Bid");
                // First findout if the same person is trying to bid on the same load twice
                logger.LogInformation("Checking if the same carrier is trying to bid on the same load twice");
                var existingBid = await repository.GetBidByLoadIdAndCarrierId(bidDto.LoadId, bidDto.CarrierId);
                
                if (existingBid != null) {
                    logger.LogError("Bid already exists for the same load and carrier");
                    return Results.Conflict("Bid already exists for the same load and carrier");
                }

                var bid = new Bid
                {
                    LoadId = bidDto.LoadId,
                    CarrierId = bidDto.CarrierId,
                    BidAmount = bidDto.BidAmount,
                    BidStatus = bidDto.BidStatus,
                    BiddingTime = DateTimeOffset.UtcNow,  // Set server-side for consistency
                    UpdatedAt = DateTimeOffset.UtcNow  // Initial set at creation
                };

                var carrier = new User
                {
                    UserId = bidDto.CarrierId,
                    FirstName = bidDto.CreatedBy.FirstName,
                    LastName = bidDto.CreatedBy.LastName,
                    Email = bidDto.CreatedBy.Email,
                    Phone = bidDto.CreatedBy.Phone,
                    MotorCarrierNumber = bidDto.CreatedBy.MotorCarrierNumber,
                    DOTNumber = bidDto.CreatedBy.DOTNumber,
                    EquipmentType = bidDto.CreatedBy.EquipmentType,
                    AvailableCapacity = bidDto.CreatedBy.AvailableCapacity,
                    CompanyName = bidDto.CreatedBy.CompanyName,
                    UserType = "carrier"
                };

                logger.LogInformation("Persisting bids in the db for carrier: {Carrier}", carrier);
                await repository.CreateBid(bid, carrier);
                logger.LogInformation("Bid Created: {Bid}", bid);
                logger.LogInformation("returing creation response for bid: {0}", bid);
                return Results.CreatedAtRoute(GetBidEndpointName, new { id = bid.Id }, bid.asDto());
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
