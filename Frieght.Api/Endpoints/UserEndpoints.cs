using Microsoft.Extensions.Logging;
using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Frieght.Api.Extensions;
using System.Linq;

namespace Frieght.Api.Endpoints
{
    public static class UserEndpoints
    {
        public class UserLogger { }

        const string GetUserEndpointName = "UserEndpoints_GetUser";
        const string GetAllUsersEndpointName = "UserEndpoints_GetAllUsers";
        const string CreateUserEndpointName = "UserEndpoints_CreateUser";
        const string UpdateUserEndpointName = "UserEndpoints_UpdateUser";
        const string DeleteUserEndpointName = "UserEndpoints_DeleteUser";

        public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/users")
                .WithParameterValidation();

            group.MapGet("/", async ([FromServices] IUserRepository repository, [FromServices] ILogger<UserLogger> logger) =>
            {

                logger.LogInformation("Fetching all users");
                var users = await repository.GetUsers();

                var result = users.Select<User, object>(user => user.UserType == "Carrier" ? (object)user.AsCarrierResponse() : (object)user.AsShipperDto());
                return Results.Ok(result);
            }).WithName(GetAllUsersEndpointName);

            group.MapGet("/{id}", async ([FromServices] IUserRepository repository, [FromServices] ILogger<UserLogger> logger, string id) =>
            {
                logger.LogInformation($"Fetching user with id {id}");
                User? user = await repository.GetUser(id);
                return user is not null ? Results.Ok(user.UserType == "Carrier" ? user.AsCarrierDto() : user.AsShipperDto()) : Results.NotFound();
            }).WithName(GetUserEndpointName);

            group.MapPost("/", async ([FromServices] IUserRepository repository, [FromServices] ILogger<UserLogger> logger, [FromServices] IValidator<CreateUserDto> validator, [FromBody] CreateUserDto createDto) =>
            {
                try
                {
                    var validationResult = await validator.ValidateAsync(createDto);
                    if (!validationResult.IsValid)
                    {
                        logger.LogWarning("Validation failed for CreateUserDto: {Errors}", validationResult.Errors);
                        return Results.BadRequest(validationResult.Errors);
                    }

                    if (!createDto.UserType.Equals("Carrier", StringComparison.OrdinalIgnoreCase) && !createDto.UserType.Equals("Shipper", StringComparison.OrdinalIgnoreCase))
                    {
                        logger.LogWarning("Invalid UserType: {UserType}", createDto.UserType);
                        return Results.BadRequest("Invalid UserType. Must be either 'Carrier' or 'Shipper'.");
                    }

                    logger.LogInformation("Creating a new user with UserId: {UserId}", createDto.UserId);

                    var user = createDto.AsUser();

                    await repository.CreateUser(user);

                    logger.LogInformation("User {UserId} created successfully", user.UserId);
                    return Results.CreatedAtRoute(GetUserEndpointName, new { id = user.UserId }, user.UserType == "Carrier" ? user.AsCarrierDto() : user.AsShipperDto());
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while creating a new user");
                    return Results.Problem("An error occurred while creating the user. Please try again later.");
                }
            }).WithName(CreateUserEndpointName);

            group.MapPut("/{id}", async ([FromServices] IUserRepository repository, [FromServices] ILogger<UserLogger> logger, [FromServices] IValidator<CreateUserDto> validator, string id, [FromBody] CreateUserDto updateDto) =>
            {
                logger.LogInformation($"Updating user with id {id}");
                User? existingUser = await repository.GetUser(id);
                if (existingUser is null)
                {
                    logger.LogWarning($"User with id {id} not found");
                    return Results.NotFound();
                }

                var validationResult = await validator.ValidateAsync(updateDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                existingUser.FirstName = updateDto.FirstName;
                existingUser.LastName = updateDto.LastName;
                existingUser.Email = updateDto.Email;
                existingUser.Phone = updateDto.Phone;
                existingUser.UserType = updateDto.UserType;

                existingUser.BusinessProfile.CompanyName = updateDto.CompanyName;
                existingUser.BusinessProfile.MotorCarrierNumber = updateDto.MotorCarrierNumber;
                existingUser.BusinessProfile.DOTNumber = updateDto.DOTNumber;
                existingUser.BusinessProfile.EquipmentType = updateDto.EquipmentType;
                existingUser.BusinessProfile.AvailableCapacity = updateDto.AvailableCapacity;
                existingUser.BusinessProfile.CarrierRole = updateDto.CarrierRole;
                existingUser.BusinessProfile.ShipperRole = updateDto.ShipperRole;

                // Update or clear BusinessVehicleTypes
                if (updateDto.VehicleTypes != null)
                {
                    existingUser.BusinessProfile.BusinessVehicleTypes = updateDto.VehicleTypes.Select(vt => new BusinessVehicleType
                    {
                        VehicleType = new VehicleType
                        {
                            Name = vt.Name,
                            Description = vt.Description,
                            ImageUrl = vt.ImageUrl,
                            VIN = vt.VIN,
                            LicensePlate = vt.LicensePlate,
                            Make = vt.Make,
                            Model = vt.Model,
                            Year = vt.Year,
                            Color = vt.Color,
                            HasInsurance = vt.HasInsurance,
                            HasRegistration = vt.HasRegistration,
                            HasInspection = vt.HasInspection
                        },
                        Quantity = vt.Quantity,
                        BusinessProfile = existingUser.BusinessProfile
                    }).ToList();
                }
                else
                {
                    existingUser.BusinessProfile.BusinessVehicleTypes.Clear();
                }

                await repository.UpdateUser(existingUser);
                logger.LogInformation($"User {id} updated successfully");
                return Results.NoContent();
            }).WithName(UpdateUserEndpointName);

            group.MapDelete("/{id}", async ([FromServices] IUserRepository repository, [FromServices] ILogger<UserLogger> logger, string id) =>
            {
                logger.LogInformation($"Deleting user with id {id}");
                User? user = await repository.GetUser(id);
                if (user != null) await repository.DeleteUser(user);

                logger.LogInformation($"User {id} deleted successfully");
                return Results.NoContent();
            }).WithName(DeleteUserEndpointName);

            return group;
        }
    }
}
