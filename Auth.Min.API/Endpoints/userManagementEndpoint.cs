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

            return groups;
        }
    }
}