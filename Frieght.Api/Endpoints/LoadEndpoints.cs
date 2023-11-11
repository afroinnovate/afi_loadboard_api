﻿using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Repositories;
using Microsoft.AspNetCore.Routing;

namespace Frieght.Api.Endpoints;

public static class LoadEndpoints
{
    const string GetLoadEndpointName = "GetLoad";

    public static RouteGroupBuilder MapLoadsEndpoints(this IEndpointRouteBuilder routes)
    {

        var groups = routes.MapGroup("/loads")
            .WithParameterValidation();

        groups.MapGet("/", async (ILoadRepository repository) => (await repository.GetLoads()).Select(load => load.asDto()));
        groups.MapGet("/{id}", async (ILoadRepository repository, int id) =>
        {
            Load? game = await repository.GetLoad(id);
            return game is not null ? Results.Ok(game.asDto()) : Results.NotFound();



        }).WithName(GetLoadEndpointName);

        groups.MapPost("/", async (ILoadRepository repository, CreateLoadDto loadDto) =>
        {
            Load load = new()
            {
                UserId = loadDto.UserId,
                Origin = loadDto.Origin,
                destination = loadDto.destination,
                PickupDate = loadDto.PickupDate,
                DeliveryDate = loadDto.DeliveryDate,
                Commodity = loadDto.Commodity,
                Weight = loadDto.Weight,
                OfferAmount = loadDto.OfferAmount,
                LoadDetails = loadDto.LoadDetails,
                LoadStatus = loadDto.LoadStatus
 
            };
            await repository.CreateLoad(load);
            return Results.CreatedAtRoute(GetLoadEndpointName, new { id = load.Id }, load);

        });

        groups.MapPut("/{id}", async (ILoadRepository repository, int id, UpdateLoadDto updatedLoadDto) =>
        {
            Load? existingLoad = await repository.GetLoad(id);
            if (existingLoad is null) return Results.NotFound();
     
            existingLoad.UserId = updatedLoadDto.UserId;
            existingLoad.Origin = updatedLoadDto.Origin;
            existingLoad.destination = updatedLoadDto.destination;
            existingLoad.PickupDate = updatedLoadDto.PickupDate;
            existingLoad.DeliveryDate = updatedLoadDto.DeliveryDate;
            existingLoad.Commodity = updatedLoadDto.Commodity;
            existingLoad.Weight = updatedLoadDto.Weight;
            existingLoad.OfferAmount = updatedLoadDto.OfferAmount;
            existingLoad.LoadDetails = updatedLoadDto.LoadDetails;
            existingLoad.LoadStatus = updatedLoadDto.LoadStatus;

            await repository.UpdateLoad(existingLoad);

            return Results.NoContent();

        });

        groups.MapDelete("/{id}", async (ILoadRepository repository, int id) =>
        {
            Load? game = await repository.GetLoad(id);
            if (game is not null) await repository.DeleteLoad(id);

            return Results.NoContent();

        });

        return groups;


    }
}


