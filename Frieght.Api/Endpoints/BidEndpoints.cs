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

    public static RouteGroupBuilder MapBidsEndpoints(this IEndpointRouteBuilder routes)
    {

        var groups = routes.MapGroup("/bids")
            .WithParameterValidation();

        groups.MapGet("/", async (IBidRepository repository) =>
            (await repository.GetBids()).Select(bid => bid.asDto()));

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

        groups.MapGet("/load/{id}", async (IBidRepository repository, int id,  ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Getting Bid by LoadId {LoadId}", id);
                var bid = await repository.GetBidByLoadId(id);
                logger.LogInformation("Bid found: {0}", bid);
                return bid != null ? Results.Ok(bid.asDto()) : Results.NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting bid by Load id {0}", id);
                return Results.Problem("An error occurred while getting bid by id", statusCode: 500);
            }
        }).WithName("GetBidByLoadId");

        groups.MapPost("/", async (IBidRepository repository, ICarrierRepository carrierRepo, CreateBidDto bidDto, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Creating Bid");
                var bid = new Bid
                {
                    LoadId = bidDto.LoadId,
                    CarrierId = bidDto.CarrierId,
                    BidAmount = bidDto.BidAmount,
                    BidStatus = bidDto.BidStatus,
                    BiddingTime = DateTimeOffset.UtcNow,  // Set server-side for consistency
                    UpdatedAt = DateTimeOffset.UtcNow  // Initial set at creation
                };

                var carrier = await carrierRepo.GetCarrier(bidDto.CarrierId);
                if (carrier == null) return Results.NotFound("Carrier not found");
                await repository.CreateBid(bid, carrier);
                logger.LogInformation("Bid Created: {Bid}", bid);
                logger.LogInformation("returing creation response for bid: {0}", bid);
                return Results.CreatedAtRoute("GetBid", new { id = bid.Id }, bid.asDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating bid");
                return Results.Problem("An error occurred while creating bid", statusCode: 500);
            }
        });

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

        return groups;
    }
}


