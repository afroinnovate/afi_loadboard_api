using Microsoft.Extensions.Logging;
using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using AutoMapper;

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

        public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder routes, IMapper mapper)
        {
            var group = routes.MapGroup("/users")
                .WithParameterValidation();

            group.MapGet("/", async ([FromServices] IUserRepository repository, [FromServices] ILogger<UserLogger> logger) =>
            {
                logger.LogInformation("Fetching all users");
                var users = await repository.GetUsers();
                var result = users.Select(user => mapper.Map<UserDto>(user));
                return Results.Ok(result);
            }).WithName(GetAllUsersEndpointName);

            group.MapGet("/{id}", async ([FromServices] IUserRepository repository, [FromServices] ILogger<UserLogger> logger, string id) =>
            {
                logger.LogInformation($"Fetching user with id {id}");
                var user = await repository.GetUser(id);
                return user is not null ? Results.Ok(mapper.Map<UserDto>(user)) : Results.NotFound();
            }).WithName(GetUserEndpointName);

            group.MapPost("/", async ([FromServices] IUserRepository repository, [FromServices] ILogger<UserLogger> logger, [FromServices] IValidator<CreateUserDto> validator, [FromBody] CreateUserDto createDto) =>
            {
                try
                {
                    logger.LogInformation("creating a new user");
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

                    // check if a user with the same UserId already exists
                    var existingUser = await repository.GetUser(createDto.UserId);
                    if (existingUser != null)
                    {
                        logger.LogWarning("User with UserId {UserId} already exists", createDto.UserId);
                        return Results.Conflict($"User with UserId {createDto.UserId} already exists");
                    }

                    var user = mapper.Map<User>(createDto);

                    await repository.CreateUser(user);

                    logger.LogInformation("User {UserId} created successfully", user.UserId);
                    var responseDto = mapper.Map<UserDto>(user);
                    return Results.CreatedAtRoute(GetUserEndpointName, new { id = user.UserId }, responseDto);
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
                var existingUser = await repository.GetUser(id);
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

                mapper.Map(updateDto, existingUser);

                await repository.UpdateUser(existingUser);
                logger.LogInformation($"User {id} updated successfully");
                return Results.NoContent();
            }).WithName(UpdateUserEndpointName);

            group.MapDelete("/{id}", async ([FromServices] IUserRepository repository, [FromServices] ILogger<UserLogger> logger, string id) =>
            {
                logger.LogInformation($"Deleting user with id {id}");
                var user = await repository.GetUser(id);
                if (user != null)
                {
                    await repository.DeleteUser(user);
                    logger.LogInformation($"User {id} deleted successfully");
                }
                return Results.NoContent();
            }).WithName(DeleteUserEndpointName);

            return group;
        }
    }
}
