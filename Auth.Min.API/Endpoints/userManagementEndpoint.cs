using System;
using System.Threading.Tasks;
using Auth.Min.API.Dtos;
using Auth.Min.API.Models;
using Auth.Min.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Auth.Min.API.Endpoints
{
    public static class UserManagementEndpoint
    {
        public class LoggerCategory
        {
            // This class is used to define the category for the logger
        }

        const string UpdateUserEndpointName = "update";
        const string GetUserEndpointName = "get";

        public static RouteGroupBuilder MapUserManagementEndpoints(this IEndpointRouteBuilder routes)
        {
            var groups = routes.MapGroup("/user");

            #region GetUserEndpoint
            groups.MapGet("/{id}", async (IUserService userService, string id, ILogger<LoggerCategory> logger) =>
            {
                try
                {
                    logger.LogInformation("Getting User by Id {Id}", id);
                    UserDto user = await userService.GetUser(id);
                    if (user == null)
                    {
                        return Results.NotFound();
                    }
                    logger.LogInformation("User found by Id {Id}", id);
                    return Results.Ok(user);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while getting user by id {Id}", id);
                    return Results.Problem("An error occurred while getting user by id", statusCode: 500);
                }
            }).WithName(GetUserEndpointName);
            #endregion

            // #region UpdateUserEndpoint
            // groups.MapPut("/{id}", async (IUserService userService, string id, UpdateUserDto dto, ILogger<LoggerCategory> logger) =>
            // {
            //     try
            //     {
            //         logger.LogInformation("Updating User by Id {Id}", id);
            //         var userDto = await userService.GetUser(id);
            //         if (userDto == null)
            //         {
            //             return Results.NotFound();
            //         }
            //         var user = userDto.ToAppUser();

            //         user.FirstName = string.IsNullOrEmpty(dto.FirstName) || dto.FirstName == "" ? userDto.FirstName : dto.FirstName;
            //         user.LastName = string.IsNullOrEmpty(dto.LastName) || dto.LastName == "" ? userDto.LastName : dto.LastName;
            //         user.MiddleName = string.IsNullOrEmpty(dto.MiddleName) || dto.MiddleName == "" ? userDto.MiddleName : dto.MiddleName;
            //         user.PhoneNumber = string.IsNullOrEmpty(dto.PhoneNumber) || dto.PhoneNumber == "" ? userDto.PhoneNumber : dto.PhoneNumber;
            //         if (!string.IsNullOrEmpty(dto.Email) && dto.Email != userDto.Email)
            //         {
            //             logger.LogInformation("Updating email");
            //             user.Email = dto.Email;
            //             user.UserName = dto.Email;
            //         }

            //         logger.LogInformation("user infos: {email}, {un}",user.Email, user.UserName);

            //         var result = await userService.UpdateUser(user);
            //         if (!result.Succeeded)
            //         {
            //             logger.LogError("An error occurred while updating user with error: {message}", result);
            //             return Results.Problem("An error occurred while updating user", statusCode: 500);
            //         }
            //         logger.LogInformation("User updated successfully");
            //         return Results.Ok(user);
            //     }
            //     catch (Exception ex)
            //     {
            //         logger.LogError(ex, "An error occurred while updating user by id {Id}", id);
            //         return Results.Problem("An error occurred while updating user by id", statusCode: 500);
            //     }
            // }).WithName(UpdateUserEndpointName);
            // #endregion

            return groups;
        }
    }
}