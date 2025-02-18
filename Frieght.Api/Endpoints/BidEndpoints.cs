﻿using AutoMapper;
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
    const string GetBidByCarrierIdEndpointName = "GetBidByCarrierId";
    const string GetBidsByLoadIdEndpointName = "GetBidsByLoadId";
    public static RouteGroupBuilder MapBidsEndpoints(this IEndpointRouteBuilder routes, IMapper mapper)
    {

        var groups = routes.MapGroup("/bids")
            .WithParameterValidation();

        #region GetBidEndpoints
        groups.MapGet("/", async (IBidRepository repository) =>
            (await repository.GetBids()).Select(bid => mapper.Map<BidDtoResponse>(bid)));
        #endregion

        #region GetBidByCarrierIdEndpoint
        groups.MapGet("/carrier/{carrierId}", async (IBidRepository repository, string carrierId, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Getting Bids by CarrierId {CarrierId}", carrierId);
                var bids = await repository.GetBidsByCarrier(carrierId);
                if (bids == null || !bids.Any()) return Results.NotFound();

                var bidDtos = new List<BidDtoResponse>();
                foreach (var bid in bids)
                {
                    var bidDto = mapper.Map<BidDtoResponse>(bid);
                    if (bid.Load != null)
                    {
                        var loadDto = mapper.Map<LoadDtoResponse>(bid.Load);
                        if (bid.Load.Shipper != null)
                        {
                            loadDto = loadDto with { CreatedBy = mapper.Map<ShipperDtoResponse>(bid.Load.Shipper) };
                        }
                        bidDto = bidDto with { Load = loadDto };
                    }
                    bidDtos.Add(bidDto);
                }
                return Results.Ok(bidDtos);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting bids by carrier id {0}", carrierId);
                return Results.Problem("An error occurred while getting bids by carrier id", statusCode: 500);
            }
        }).WithName(GetBidByCarrierIdEndpointName);
        #endregion

        #region GetBidByIdEndpoint
        groups.MapGet("/{id:int}", async (IBidRepository repository, int id, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Getting Bid by Id {Id}", id);
                var bid = await repository.GetBid(id);
                if (bid == null) logger.LogInformation("Bid not found with Id {Id}", id);
                var bidDto = mapper.Map<BidDtoResponse>(bid);

                if (bidDto.Load != null)
                {
                    var loadDto = mapper.Map<LoadDtoResponse>(bid.Load);
                    if (bid.Load.Shipper != null)
                    {
                        loadDto = loadDto with { CreatedBy = mapper.Map<ShipperDtoResponse>(bid.Load.Shipper) };
                    }
                    bidDto = bidDto with { Load = loadDto };
                }

                return bidDto != null ? Results.Ok(bidDto) : Results.NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting bid by id {0}", id);
                return Results.Problem("An error occurred while getting bid by id", statusCode: 500);
            }
        }).WithName(GetBidEndpointName);
        #endregion

        #region GetBidsByLoadIdEndpoint
        groups.MapGet("/load/{loadId}", async (IBidRepository repository, int loadId, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                IEnumerable<BidDtoResponse> bidDtoResponses = new List<BidDtoResponse>();

                logger.LogInformation("Getting Bids by LoadId {LoadId}", loadId);
                IEnumerable<Bid?> bids = await repository.GetBidsByLoadId(loadId);
                foreach (var bid in bids)
                {
                    var bidDto = mapper.Map<BidDtoResponse>(bid);
                    bidDtoResponses = bidDtoResponses.Append(bidDto);
                }
                logger.LogInformation("Retrieved {BidCount} bids by LoadId {LoadId}", bids.Count(), loadId);
                return Results.Ok(bidDtoResponses);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting bids by load id {0}", loadId);
                return Results.Problem("An error occurred while getting bids by load id", statusCode: 500);
            }
        }).WithName(GetBidsByLoadIdEndpointName);
        #endregion

        #region GetBidByLoadAndCarrierEndpoint
        groups.MapGet("/{loadId}/{carrierId}", async (IBidRepository repository, int loadId, string carrierId, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Getting Bid by Load Id and carrier id {loadId} with carrierId {carrierId}", loadId, carrierId);
                var bid = await repository.GetBidByLoadIdAndCarrierId(loadId, carrierId);
                return bid != null ? Results.Ok(mapper.Map<BidDtoResponse>(bid)) : Results.NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting bid by load id {0} and carrierId {1}", loadId, carrierId);
                return Results.Problem("An error occurred while getting bid by id", statusCode: 500);
            }
        }).WithName(GetBidByLoadAndCarrierEndpointName);
        #endregion
        
        #region CreateBidEndpoint
        groups.MapPost("/", async (IBidRepository repository, IUserRepository userRepo, ILoadRepository loadRepo, IMapper mapper, CreateBidDto bidDto, ILogger<LoggerCategory> logger) =>
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
                            DOTNumber = bidDto.CreatedBy.DotNumber,
                            MotorCarrierNumber = bidDto.CreatedBy.MotorCarrierNumber,
                            EquipmentType = bidDto.CreatedBy.EquipmentType,
                            AvailableCapacity = bidDto.CreatedBy.AvailableCapacity,
                            CarrierRole = bidDto.CreatedBy.CarrierRole,
                            CarrierVehicles = new List<Vehicle>
                            {
                                new Vehicle
                                {
                                    Name = bidDto.CreatedBy.Name,
                                    Description = bidDto.CreatedBy.Description,
                                    ImageUrl = bidDto.CreatedBy.ImageUrl,
                                    VIN = bidDto.CreatedBy.Vin,
                                    LicensePlate = bidDto.CreatedBy.LicensePlate,
                                    Make = bidDto.CreatedBy.Make,
                                    Model = bidDto.CreatedBy.Model,
                                    Year = bidDto.CreatedBy.Year,
                                    Color = bidDto.CreatedBy.Color,
                                    HasInsurance = bidDto.CreatedBy.HasInsurance,
                                    HasRegistration = bidDto.CreatedBy.HasRegistration,
                                    HasInspection = bidDto.CreatedBy.HasInspection
                                }
                            }
                        }
                    };
                }

                var bid = mapper.Map<Bid>(bidDto);
                bid.Load = load;
                bid.Carrier = trackedCarrier;

                // Set timestamps
                bid.BiddingTime = DateTimeOffset.UtcNow;
                bid.UpdatedAt = DateTimeOffset.UtcNow;

                // Check if a load is already bid on by the carrier
                logger.LogInformation("Checking if bid already exists for LoadId {LoadId} and CarrierId {CarrierId}", bidDto.LoadId, bidDto.CarrierId);
                var existingBid = await repository.GetBidByLoadIdAndCarrierId(bidDto.LoadId, bidDto.CarrierId);
                if (existingBid != null)
                {
                    logger.LogWarning("Bid already exists for LoadId {LoadId} and CarrierId {CarrierId}", bidDto.LoadId, bidDto.CarrierId);
                    return Results.Conflict("Bid already exists for LoadId and CarrierId");
                }

                await repository.CreateBid(bid);
                logger.LogInformation("Bid created successfully with BidId: {BidId}", bid.Id);
                var response = mapper.Map<BidDtoResponse>(bid);

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

                logger.LogInformation("Retrieving Bid by Id {Id}", id);
                var existingBid = await repository.GetBid(id);
                if (existingBid == null){
                    logger.LogInformation("Bid not found with Id {Id}", id);
                    return Results.NotFound($"Bid with Id: {id} not found");
                }

                if (updatedBidDto.UpdatedBy == null || updatedBidDto.UpdatedBy == "" || updatedBidDto.UpdatedBy == "string")
                {
                    logger.LogWarning("UpdatedBy is required to update bid");
                    return Results.BadRequest("UpdatedBy is required to update bid");
                }

                existingBid.LoadId = existingBid.LoadId;
                existingBid.CarrierId = existingBid.CarrierId;
                existingBid.BidAmount = updatedBidDto.BidAmount == 0 ? existingBid.BidAmount : updatedBidDto.BidAmount;
                existingBid.BidStatus = updatedBidDto.BidStatus == 0 ? existingBid.BidStatus : updatedBidDto.BidStatus;
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
