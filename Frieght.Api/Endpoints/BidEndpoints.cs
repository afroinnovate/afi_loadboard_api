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

        groups.MapGet("/", async (IBidRepository repository) => (await repository.GetBids()).Select(bid => bid.asDto()));
        groups.MapGet("/{id}", async (IBidRepository repository, int id,  ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Getting Bid by Id {0}", id);

                Bid? bid = await repository.GetBid(id);
                logger.LogInformation("Bid found: {0}", bid);
                return bid is not null ? Results.Ok(bid.asDto()) : Results.NotFound();
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
                logger.LogInformation("Getting Bid by loadId {0}", id);

                Bid? bid = await repository.GetBidByLoadId(id);
                logger.LogInformation("Bid found: {0}", bid);
                return bid is not null ? Results.Ok(bid.asDto()) : Results.NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting bid by Load id {0}", id);
                return Results.Problem("An error occurred while getting bid by id", statusCode: 500);
            }
        }).WithName("GetBidByLoadId");

        groups.MapPost("/", async (IBidRepository repository, CreateBidDto bidDto, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Creating Bid");
                Bid bid = new()
                {
                    LoadId = bidDto.LoadId,
                    CarrierId = bidDto.CarrierId,
                    BidAmount = bidDto.BidAmount,
                    BidStatus = bidDto.BidStatus,
                };
                logger.LogInformation("Bid created: {0}", bid);
                await repository.CreateBid(bid);
                logger.LogInformation("returing creation response for bid: {0}", bid);
                return Results.CreatedAtRoute(GetBidEndpointName, new { id = bid.Id }, bid);
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
                logger.LogInformation("Updating Bid by Id {0}", id);
                Bid? existingBid = await repository.GetBid(id);
                if (existingBid is null) return Results.NotFound();
        
                existingBid.LoadId = updatedBidDto.LoadId;
                existingBid.CarrierId = updatedBidDto.CarrierId;
                existingBid.BidAmount = updatedBidDto.BidAmount;
                existingBid.BidStatus = updatedBidDto.BidStatus;  
                existingBid.UpdatedAt = updatedBidDto.UpdatedAt;
                existingBid.UpdatedBy = updatedBidDto.UpdatedBy;

                logger.LogInformation("Bid updated: {0}", existingBid);
                await repository.UpdateBid(existingBid);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating bid by id {0}", id);
                return Results.Problem("An error occurred while updating bid", statusCode: 500);
            }
        });

        groups.MapDelete("/{id}", async (IBidRepository repository, int id,  ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Deleting Bid by Id {0}", id);
                Bid? bid = await repository.GetBid(id);
                if (bid is not null) await repository.DeleteBid(id);
                logger.LogInformation("Bid deleted: {0}", bid);
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


