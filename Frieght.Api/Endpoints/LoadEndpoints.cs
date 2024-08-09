using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Infrastructure.Exceptions;
using Frieght.Api.Infrastructure.Notifications;
using Frieght.Api.Repositories;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;

namespace Frieght.Api.Endpoints;

public static class LoadEndpoints
{
    public class LoggerCategory
    {
        // This class is used to define the category for the logger
    }
    const string GetLoadEndpointName = "GetLoad";

    public static RouteGroupBuilder MapLoadsEndpoints(this IEndpointRouteBuilder routes, IMapper mapper)
    {
        var groups = routes.MapGroup("/loads")
            .WithParameterValidation();

        #region GetLoad
        /// <summary>
        /// Get all loads
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        groups.MapGet("/", async (ILoadRepository repository, IMapper mapper, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Retrieving all loads from the repository.");

                var loads = await repository.GetLoads();

                if (loads == null || !loads.Any())
                {
                    logger.LogInformation("No loads found.");
                    return Results.Ok(new List<LoadDtoResponse>());
                }

                logger.LogInformation("Mapping loads to LoadDtoResponse.");

                var loadDtos = loads.Select(load =>
                {
                    if (load.Shipper == null)
                    {
                        logger.LogWarning("Load {LoadId} has no Shipper associated with it.", load.LoadId);
                    }
                    else
                    {
                        logger.LogInformation("Shipping found for ShipperId {ShipperId}.", load.Shipper.UserId);
                    }

                    // Map the Load entity to LoadDtoResponse
                    var mappedDto = mapper.Map<LoadDtoResponse>(load);

                    // Reinitialize the LoadDtoResponse with CreatedBy set
                    var loadDtoWithCreatedBy = mappedDto with { CreatedBy = mapper.Map<ShipperDtoResponse>(load.Shipper) };

                    if (loadDtoWithCreatedBy.CreatedBy == null)
                    {
                        logger.LogWarning("Mapped LoadDtoResponse {LoadId} has a null CreatedBy.", load.LoadId);
                    }

                    return loadDtoWithCreatedBy;
                });

                logger.LogInformation("Successfully mapped {Count} loads.", loadDtos.Count());

                return Results.Ok(loadDtos);
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
        groups.MapGet("/{id}", async (ILoadRepository repository, int id, IMapper mapper, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                var load = await repository.GetLoad(id);
                if (load == null)
                {
                    return Results.NotFound();
                }

                // Map the Load entity to LoadDtoResponse
                var loadDto = mapper.Map<LoadDtoResponse>(load);

                // Reinitialize the LoadDtoResponse with CreatedBy set
                var loadDtoWithCreatedBy = loadDto with { CreatedBy = mapper.Map<ShipperDtoResponse>(load.Shipper) };

                logger.LogInformation("Load found: {0}", loadDtoWithCreatedBy);
                return Results.Ok(loadDtoWithCreatedBy);
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
        groups.MapPost("/", async (ILoadRepository repository, CreateLoadDto loadDto, IUserRepository userRepository, IMessageSender messageSender, ILogger<LoggerCategory> logger) =>
        {
            logger.LogInformation("Creating Load");
            try
            {
                logger.LogInformation("Creating Load");

                // Map the DTO to the Load entity
                var load = mapper.Map<Load>(loadDto);

                // Create the load with the Shipper (CreatedBy in the DTO)
                await repository.CreateLoad(load, mapper.Map<User>(loadDto.CreatedBy));
                logger.LogInformation("Load Created Successfully");

                // TODO: Uncomment the following code to notify carriers and implement the NotifyCarriers method
                // This should be done by getting User info from the User table and use them to Notify the carrier.

                // logger.LogInformation("Notifying Carriers");

                // await NotifyCarriers(carrierRepository, messageSender, load, logger);
                // logger.LogInformation("Carriers Notified");

                var createdLoadDto = mapper.Map<LoadDtoResponse>(load);
                return Results.CreatedAtRoute(GetLoadEndpointName, new { id = load.LoadId }, createdLoadDto);
            }
            catch (DuplicateLoadException ex)
            {
                logger.LogWarning(ex, "Duplicate load creation attempt.");
                return Results.BadRequest(ex.Message);
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
        async Task<string> NotifyCarriers(IUserRepository userRepository, IMessageSender messageSender, Load load, ILogger<LoggerCategory> logger)
        {
            logger.LogInformation("Getting Carriers");
            IEnumerable<User> carriers = await userRepository.GetUserByUserType("carrier");
            if (carriers is not null)
            {
                logger.LogInformation("Carriers found: {0}", carriers.Count());
                foreach (var carrier in carriers)
                {
                    try
                    {
                        await messageSender.SendEmailAsync(carrier.Email, $"From {load.Origin} to {load.Destination}", load.LoadDetails);
                        logger.LogInformation("Carrier {0} notified", carrier.BusinessProfile?.CompanyName);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred while notifying carrier {0}", carrier.BusinessProfile?.CompanyName);
                    }
                }
            }
            return "All carriers have been notified";
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
        groups.MapPut("/{id}", async (ILoadRepository repository, int id, UpdateLoadDto updateLoadDto, ILogger<LoggerCategory> logger) =>
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

                // Map the incoming DTO to the existing entity
                mapper.Map(updateLoadDto, existingLoad);

                existingLoad.ModifiedAt = DateTime.UtcNow;
                existingLoad.ModifiedBy = updateLoadDto.ModifiedBy;

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
