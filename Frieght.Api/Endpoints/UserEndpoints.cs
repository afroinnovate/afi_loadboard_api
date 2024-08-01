using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Repositories;
using Microsoft.AspNetCore.Routing;

namespace Frieght.Api.Endpoints
{
    public static class UserEndpoints
    {
        const string GetUserEndpointName = "GetUser";

        public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/users")
                .WithParameterValidation();

            group.MapGet("/", async (IUserRepository repository) =>
            {
                var users = await repository.GetUsers();
                var result = users.Select<User, object>(user => user.UserType == "Carrier" ? user.asCarrierDto() : user.asShipperDto());
                return Results.Ok(result);
            });

            group.MapGet("/{id}", async (IUserRepository repository, string id) =>
            {
                User? user = await repository.GetUser(id);
                return user is not null ? Results.Ok(user.UserType == "Carrier" ? user.asCarrierDto() : user.asShipperDto()) : Results.NotFound();
            }).WithName(GetUserEndpointName);

            group.MapPost("/", async (IUserRepository repository, CreateUserDto createDto) =>
            {
                User user = new()
                {
                    UserId = createDto.UserId,
                    FirstName = createDto.FirstName,
                    LastName = createDto.LastName,
                    Email = createDto.Email,
                    Phone = createDto.Phone,
                    UserType = createDto.UserType
                };

                // Initialize BusinessProfile based on UserType
                user.BusinessProfile = createDto.UserType == "Carrier" ? new BusinessProfile
                {
                    UserId = createDto.UserId, // Set UserId here
                    CompanyName = createDto.CompanyName,
                    MotorCarrierNumber = createDto.MotorCarrierNumber,
                    DOTNumber = createDto.DOTNumber,
                    EquipmentType = createDto.EquipmentType,
                    AvailableCapacity = createDto.AvailableCapacity,
                    CarrierRole = createDto.CarrierRole,
                    User = user // Set User here to establish relationship
                } : new BusinessProfile
                {
                    UserId = createDto.UserId, // Set UserId here
                    CompanyName = createDto.CompanyName,
                    DOTNumber = createDto.DOTNumber,
                    ShipperRole = createDto.ShipperRole,
                    User = user // Set User here to establish relationship
                };

                await repository.CreateUser(user);
                return Results.CreatedAtRoute(GetUserEndpointName, new { id = user.UserId }, user);
            });

            group.MapPut("/{id}", async (IUserRepository repository, string id, UpdateUserDto updateDto) =>
            {
                User? existingUser = await repository.GetUser(id);
                if (existingUser is null) return Results.NotFound();

                existingUser.FirstName = updateDto.FirstName;
                existingUser.LastName = updateDto.LastName;
                existingUser.Email = updateDto.Email;
                existingUser.Phone = updateDto.Phone;
                existingUser.UserType = updateDto.UserType;

                if (existingUser.BusinessProfile != null)
                {
                    existingUser.BusinessProfile.CompanyName = updateDto.CompanyName;
                    existingUser.BusinessProfile.MotorCarrierNumber = updateDto.MotorCarrierNumber;
                    existingUser.BusinessProfile.DOTNumber = updateDto.DOTNumber;
                    existingUser.BusinessProfile.EquipmentType = updateDto.EquipmentType;
                    existingUser.BusinessProfile.AvailableCapacity = updateDto.AvailableCapacity;
                    existingUser.BusinessProfile.CarrierRole = updateDto.CarrierRole;
                    existingUser.BusinessProfile.ShipperRole = updateDto.ShipperRole;
                }
                else
                {
                    existingUser.BusinessProfile = updateDto.UserType == "Carrier" ? new BusinessProfile
                    {
                        UserId = existingUser.UserId, // Set UserId here
                        User = existingUser, // Set User here
                        CompanyName = updateDto.CompanyName,
                        MotorCarrierNumber = updateDto.MotorCarrierNumber,
                        DOTNumber = updateDto.DOTNumber,
                        EquipmentType = updateDto.EquipmentType,
                        AvailableCapacity = updateDto.AvailableCapacity,
                        CarrierRole = updateDto.CarrierRole
                    } : new BusinessProfile
                    {
                        UserId = existingUser.UserId, // Set UserId here
                        User = existingUser, // Set User here
                        CompanyName = updateDto.CompanyName,
                        DOTNumber = updateDto.DOTNumber,
                        ShipperRole = updateDto.ShipperRole
                    };
                }

                await repository.UpdateUser(existingUser);
                return Results.NoContent();
            });

            group.MapDelete("/{id}", async (IUserRepository repository, string id) =>
            {
                User? user = await repository.GetUser(id);
                if (user != null) await repository.DeleteUser(user);

                return Results.NoContent();
            });

            return group;
        }
    }
}
