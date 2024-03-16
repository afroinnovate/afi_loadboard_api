using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Repositories;
using Microsoft.AspNetCore.Routing;

namespace Frieght.Api.Endpoints;

public static class BidEndpoints
{
    const string GetBidEndpointName = "GetBid";

    public static RouteGroupBuilder MapBidsEndpoints(this IEndpointRouteBuilder routes)
    {

        var groups = routes.MapGroup("/bids")
            .WithParameterValidation();

        groups.MapGet("/", async (IBidRepository repository) => (await repository.GetBids()).Select(bid => bid.asDto()));
        groups.MapGet("/{id}", async (IBidRepository repository, int id) =>
        {
            Bid? bid = await repository.GetBid(id);
            return bid is not null ? Results.Ok(bid.asDto()) : Results.NotFound();



        }).WithName(GetBidEndpointName);

        groups.MapPost("/", async (IBidRepository repository, CreateBidDto bidDto) =>
        {
            Bid bid = new()
            {
                LoadId = bidDto.LoadId,
                CarrierId = bidDto.CarrierId,
                BidAmount = bidDto.BidAmount,
                BidStatus = bidDto.BidStatus,

 
            };
            await repository.CreateBid(bid);
            return Results.CreatedAtRoute(GetBidEndpointName, new { id = bid.Id }, bid);

        });

        groups.MapPut("/{id}", async (IBidRepository repository, int id, UpdateBidDto updatedBidDto) =>
        {
            Bid? existingBid = await repository.GetBid(id);
            if (existingBid is null) return Results.NotFound();
     
            existingBid.LoadId = updatedBidDto.LoadId;
            existingBid.CarrierId = updatedBidDto.CarrierId;
            existingBid.BidAmount = updatedBidDto.BidAmount;
            existingBid.BidStatus = updatedBidDto.BidStatus;  
            existingBid.UpdatedAt = updatedBidDto.UpdatedAt;
            existingBid.UpdatedBy = updatedBidDto.UpdatedBy;

            await repository.UpdateBid(existingBid);

            return Results.NoContent();

        });

        groups.MapDelete("/{id}", async (IBidRepository repository, int id) =>
        {
            Bid? bid = await repository.GetBid(id);
            if (bid is not null) await repository.DeleteBid(id);

            return Results.NoContent();

        });

        return groups;


    }
}


