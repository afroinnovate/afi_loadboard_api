﻿using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Infrastructure.Notifications;
using Frieght.Api.Repositories;
using Microsoft.AspNetCore.Routing;
using Microsoft.IdentityModel.Tokens;

namespace Frieght.Api.Endpoints;

public static class LoadEndpoints
{
    const string GetLoadEndpointName = "GetLoad";

    public static RouteGroupBuilder MapLoadsEndpoints(this IEndpointRouteBuilder routes)
    {

        var groups = routes.MapGroup("/loads")
            .WithParameterValidation();

        
        /// <summary>
        /// Get all loads
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        groups.MapGet("/", async (ILoadRepository repository) => (await repository.GetLoads()).Select(load => load.asDto()));

        /// <summary>
        ///  Get Load by Id
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        groups.MapGet("/{id}", async (ILoadRepository repository, int id) =>
        {
            Load? load = await repository.GetLoad(id);
            
            return load is not null ? Results.Ok(load.asDto()) : Results.NotFound();

        }).WithName(GetLoadEndpointName);

        /// <summary>
        /// Post a load
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="loadDto"></param>
        /// <param name="carrierRepository"></param>
        /// <param name="messageSender"></param>
        /// <returns></returns>
        groups.MapPost("/", async (ILoadRepository repository, CreateLoadDto loadDto,
            ICarrierRepository carrierRepository, IMessageSender messageSender) =>
        {
            Load load = new()
            {
                UserId = loadDto.UserId,
                ShipperUserId = loadDto.ShipperUserId,
                Origin = loadDto.Origin,
                destination = loadDto.destination,
                PickupDate = loadDto.PickupDate,
                DeliveryDate = loadDto.DeliveryDate,
                Commodity = loadDto.Commodity,
                Weight = loadDto.Weight,
                OfferAmount = loadDto.OfferAmount,
                LoadDetails = loadDto.LoadDetails,
                LoadStatus = loadDto.LoadStatus,
                Created = DateTime.UtcNow
 
            };
            await repository.CreateLoad(load);
            
            //Notify Carrier of new load posting
            await NotifyCarriers(carrierRepository, messageSender, load);

            return Results.CreatedAtRoute(GetLoadEndpointName, new { id = load.Id }, load);

        });

        async Task<string> NotifyCarriers(ICarrierRepository carrierRepository, IMessageSender messageSender, Load load)
        {
            var carriers = await carrierRepository.GetCarriers();
            if(carriers is not null)
            {
                foreach (var carrier in carriers)
                {
                   
                    await messageSender.SendEmailAsync(carrier.CompanyEmail, $"From {load.Origin} to {load.destination}", load.LoadDetails);
                }
            }


            return  "All carriers have been notified";
        }

        /// <summary>
        /// Update load
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="id"></param>
        /// <param name="updatedLoadDto"></param>
        /// <returns></returns>
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
            existingLoad.Modified = DateTime.UtcNow;
            existingLoad.ModifiedBy = updatedLoadDto.ModifiedBy;

            await repository.UpdateLoad(existingLoad);

            return Results.NoContent();

        });

        /// <summary>
        /// Delete load by ID
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        groups.MapDelete("/{id}", async (ILoadRepository repository, int id) =>
        {
            Load? load = await repository.GetLoad(id);
            if (load is not null) await repository.DeleteLoad(id);

            return Results.NoContent();

        });

        return groups;
    }
}


