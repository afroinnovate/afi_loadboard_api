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

        #region GetLoad
        /// <summary>
        /// Get all loads
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        groups.MapGet("/", async (ILoadRepository repository, ILogger<LoggerCategory> logger) => 
        {
            try
            {
                var loads = await repository.GetLoads();
                return Results.Ok(loads.Select(load => load.asDto()));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve all loads");
                return Results.Problem("An error occurred while retrieving loads", statusCode: 500);
            }
        });
        #endregion

        #region GetLoadById
        /// <summary>
        ///  Get Load by Id
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        groups.MapGet("/{id}", async (ILoadRepository repository, int id, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                var load = await repository.GetLoad(id);
                logger.LogInformation("Load found: {0}", load);
                return load != null ? Results.Ok(load.asDto()) : Results.NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve load by Id {Id}", id);
                return Results.Problem("An error occurred while retrieving the load", statusCode: 500);
            }
        }).WithName(GetLoadEndpointName);
        #endregion

        #region PostLoad
        /// <summary>
        /// Post a load
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="loadDto"></param>
        /// <param name="carrierRepository"></param>
        /// <param name="messageSender"></param>
        /// <returns></returns>
        groups.MapPost("/", async (ILoadRepository repository, CreateLoadDto loadDto, ICarrierRepository carrierRepository, IMessageSender messageSender, ILogger<LoggerCategory> logger) =>
        {
            logger.LogInformation("Creating Load");
            try
            {
                var load = new Load
                {
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
                    CreatedAt = DateTime.UtcNow,
                    Shipper = new User
                    {
                        UserId = loadDto.ShipperUserId,
                        Email = loadDto.CreatedBy.Email,
                        CompanyName = loadDto.CreatedBy.CompanyName,
                        DOTNumber = loadDto.CreatedBy.DOTNumber,
                        FirstName = loadDto.CreatedBy.FirstName,
                        LastName = loadDto.CreatedBy.LastName,
                    }
                };

                var shipper = new User
                {
                    UserId = loadDto.ShipperUserId,
                    Email = loadDto.CreatedBy.Email,
                    CompanyName = loadDto.CreatedBy.CompanyName,
                    DOTNumber = loadDto.CreatedBy.DOTNumber,
                    FirstName = loadDto.CreatedBy.FirstName,
                    LastName = loadDto.CreatedBy.LastName,
                    UserType = "shipper"
                };

                await repository.CreateLoad(load, shipper);
                logger.LogInformation("Load Created Successfully");

                // TODO: Uncomment the following code to notify carriers and implemente the NotifyCarriers method
                // This should be done by getting User info from the User table and use them to Notify the carrier.

                // logger.LogInformation("Notifying Carriers");
           
                // await NotifyCarriers(carrierRepository, messageSender, load, logger);
                // logger.LogInformation("Carriers Notified");
            
                return Results.CreatedAtRoute(GetLoadEndpointName, new { id = load.LoadId }, load.asDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating load");
                return Results.Problem("An error occurred while creating load", statusCode: 500);
            }

        });
        #endregion

        #region NotifyCarriers
        /// <summary>
        /// Notify carriers
        /// </summary>
        /// <param name="carrierRepository"></param>
        /// <param name="messageSender"></param>
        /// <param name="load"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        async Task<string> NotifyCarriers(ICarrierRepository carrierRepository, IMessageSender messageSender, Load load, ILogger<LoggerCategory> logger)
        {
            logger.LogInformation("Getting Carriers");
            var carriers = await carrierRepository.GetCarrierByUserType("carrier");
            if(carriers is not null)
            {
                logger.LogInformation("Carriers found: {0}", carriers.Count());
                foreach (var carrier in carriers)
                {
                   try 
                   {
                        await messageSender.SendEmailAsync(carrier.Email, $"From {load.Origin} to {load.Destination}", load.LoadDetails);
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
        #endregion

        #region UpdateLoad
        /// <summary>
        /// Update load
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="id"></param>
        /// <param name="updatedLoadDto"></param>
        /// <returns></returns>
        groups.MapPut("/{id}", async (ILoadRepository repository, int id, UpdateLoadDto updatedLoadDto, ILogger<LoggerCategory> logger) =>
        {
            logger.LogInformation("Updating Load with ID: {Id}", id);
            try
            {
                var existingLoad = await repository.GetLoad(id);
                if (existingLoad == null)
                {
                    logger.LogWarning("Load with ID: {Id} not found", id);
                    return Results.NotFound("Load not found");
                }

                // Map updatedLoadDto to existingLoad
                existingLoad.ShipperUserId = updatedLoadDto.ShipperUserId;
                existingLoad.Origin = updatedLoadDto.Origin;
                existingLoad.Destination = updatedLoadDto.Destination;
                existingLoad.PickupDate = updatedLoadDto.PickupDate;
                existingLoad.DeliveryDate = updatedLoadDto.DeliveryDate;
                existingLoad.Commodity = updatedLoadDto.Commodity;
                existingLoad.Weight = updatedLoadDto.Weight;
                existingLoad.OfferAmount = updatedLoadDto.OfferAmount;
                existingLoad.LoadDetails = updatedLoadDto.LoadDetails;
                existingLoad.LoadStatus = updatedLoadDto.LoadStatus;
                existingLoad.ModifiedAt = DateTime.UtcNow;
                existingLoad.ModifiedBy = updatedLoadDto.ModifiedBy;

                await repository.UpdateLoad(existingLoad);
                logger.LogInformation("Load with ID: {Id} updated successfully", id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update Load with ID: {Id}", id);
                return Results.Problem("An error occurred while updating the load", statusCode: 500);
            }
        });
        #endregion

        #region DeleteLoad
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
                var load = await repository.GetLoad(id);
                if (load == null) return Results.NotFound();
                await repository.DeleteLoad(id);
                logger.LogInformation("Load Deleted: {0}", id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete load by Id {Id}", id);
                return Results.Problem("An error occurred while deleting the load", statusCode: 500);
            }

        });
        #endregion

        return groups;
    }
}
