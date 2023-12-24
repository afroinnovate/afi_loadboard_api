
using System.Security.Claims;
using Auth.Min.API.Dtos;
using Auth.Min.API.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;

namespace Auth.Min.API.Endpoints;
public static class AuthEndpoints
{

    const string GetAuthEndpointName = "auth";
    public static void AddRegistrationEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/register", async (RegistrationModel model,UserManager<AppUser> userManager) =>
        { 
            // Handle user registration logic here
            var user = new AppUser { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Results.BadRequest(new { Errors = errors });
            }

            return Results.Ok("User created successfully");
        })
        .WithName("RegisterUser")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }

    

    public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {

        var groups = routes.MapGroup("/auth").WithName(GetAuthEndpointName);

        groups.MapGet("/Health", () => "OK from Auth.Min.API");

        // Add registration endpoints
        groups.AddRegistrationEndpoints();

       return groups;
    }
}
