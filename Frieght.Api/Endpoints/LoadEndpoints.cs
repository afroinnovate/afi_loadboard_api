using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Infrastructure.Notifications;
using Frieght.Api.Repositories;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;

namespace Frieght.Api.Endpoints;

public static class LoadEndpoints
{
    public class LoggerCategory 
    {
        // This class is used to define the category for the logger
    }
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
        groups.MapGet("/", async (ILoadRepository repository, ILogger<LoggerCategory> logger) => 
        {
            logger.LogInformation("Getting all loads");
            try
            {
                var loads = await repository.GetLoads();
                logger.LogInformation("{0} Loads found", loads.Count());
                return Results.Ok(loads.Select(load => load.asDto()));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting all loads");
                return Results.Problem("An error occurred while getting all loads", statusCode: 500);
            }
        });

        /// <summary>
        ///  Get Load by Id
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        groups.MapGet("/{id}", async (ILoadRepository repository, int id, ILogger<LoggerCategory> logger) =>
        {
            logger.LogInformation("Getting Load by Id {0}", id);
            try
            {
                Load? load = await repository.GetLoad(id);
                logger.LogInformation("Load found: {0}", load);
                return load is not null ? Results.Ok(load.asDto()) : Results.NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting Load by Id {0}", id);
                return Results.Problem("An error occurred while getting Load by Id", statusCode: 500);
            }

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
            ICarrierRepository carrierRepository, IMessageSender messageSender, ILogger<LoggerCategory> logger) =>
        {
            logger.LogInformation("Creating Load");
            try
            {
                await repository.CreateLoad(loadDto);
                logger.LogInformation("Load Created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating Load");
                return Results.Problem("An error occurred while creating Load", statusCode: 500);
            }
            
            //Notify Carrier of new load posting
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

            logger.LogInformation("Notifying Carriers");
            try
            {
                await NotifyCarriers(carrierRepository, messageSender, load, logger);
                logger.LogInformation("Carriers Notified");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while notifying carriers");
                return Results.Problem("An error occurred while notifying carriers", statusCode: 500);
            }

            return Results.CreatedAtRoute(GetLoadEndpointName, new { id = load.Id }, load);

        });

        async Task<string> NotifyCarriers(ICarrierRepository carrierRepository, IMessageSender messageSender, Load load, ILogger<LoggerCategory> logger)
        {
            logger.LogInformation("Getting Carriers");
            var carriers = await carrierRepository.GetCarriers();
            if(carriers is not null)
            {
                logger.LogInformation("Carriers found: {0}", carriers.Count());
                foreach (var carrier in carriers)
                {
                   try 
                   {
                        await messageSender.SendEmailAsync(carrier.CompanyEmail, $"From {load.Origin} to {load.Destination}", load.LoadDetails);
                        logger.LogInformation("Carrier {0} notified", carrier.CompanyName);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred while notifying carrier {0}", carrier.CompanyName);
                    }
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
        groups.MapPut("/{id}", async (ILoadRepository repository, int id, UpdateLoadDto updatedLoadDto, ILogger<LoggerCategory> logger) =>
        {
            logger.LogInformation("Updating Load");
            try
            {
                Load? existingLoad = await repository.GetLoad(id);
                if (existingLoad is null) return Results.NotFound();
        
                existingLoad.UserId = updatedLoadDto.UserId;
                existingLoad.Origin = updatedLoadDto.Origin;
                existingLoad.Destination = updatedLoadDto.Destination;
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
                logger.LogInformation("Load Updated");
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating Load");
                return Results.Problem("An error occurred while updating Load", statusCode: 500);
            }
        });

        /// <summary>
        /// Delete load by ID
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        groups.MapDelete("/{id}", async (ILoadRepository repository, int id, ILogger<LoggerCategory> logger) =>
        {
            logger.LogInformation("Deleting Load");
            try
            {
                Load? load = await repository.GetLoad(id);
                if (load is not null) await repository.DeleteLoad(id);
                logger.LogInformation("Load Deleted");
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting Load");
                return Results.Problem("An error occurred while deleting Load", statusCode: 500);
            }

        });

        return groups;
    }
}


