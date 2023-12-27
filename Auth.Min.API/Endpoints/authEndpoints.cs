
using System.Security.Claims;
using Auth.Min.API.Dtos;
using Auth.Min.API.Models;
using Auth.Min.API.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Auth.Min.API.Endpoints;
public static class AuthEndpoints
{
    const string GetAuthEndpointName = "auth";

    public static void AddRegistrationEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/register", async (RegistrationModel model, UserManager<AppUser> userManager) =>
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

    public static void AddCompleteProfileEndpoint(this RouteGroupBuilder group, Roles roleConfig)
    {
        group.MapPost("/completeprofile", async (CompleteProfileRequest request, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) => 
        {
            // Find the user
            var user = await userManager.FindByEmailAsync(request.Username);
            if (user == null)
            {
                return Results.NotFound("User not Found");
            }

            // Update user details
            user.FirstName = string.IsNullOrEmpty(request.FirstName) ? user.FirstName : request.FirstName;
            user.LastName = string.IsNullOrEmpty(request.LastName) ? user.LastName : request.LastName;
            user.DotNumber = string.IsNullOrEmpty(request.DotNumber) ? user.DotNumber : request.DotNumber;
            user.CompanyName = string.IsNullOrEmpty(request.CompanyName) ? user.CompanyName : request.CompanyName;

            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return Results.BadRequest(updateResult.Errors);
            }

            // Assign the role to the user
            if (!string.IsNullOrEmpty(request.Role))
            {
                // check if the roles is valid 
                 // Check if the role is in the predefined roles
                if (!IsRoleValid(request.Role, roleConfig))
                {
                    return Results.BadRequest("Invalid role.");
                }

                // Check if the role exists in the database, create if not
                if (!await roleManager.RoleExistsAsync(request.Role))
                {
                    await roleManager.CreateAsync(new IdentityRole(request.Role));
                }

                // Check if the role is valid
                if (!await roleManager.RoleExistsAsync(request.Role) )
                {
                    return Results.BadRequest("Invalid role.");
                }

                var roleResult = await userManager.AddToRoleAsync(user, request.Role);
                if (!roleResult.Succeeded)
                {
                    return Results.BadRequest(roleResult.Errors);
                }
            }

            return Results.Ok("User profile updated successfully");
        }).WithName("CompleteProfile")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }

    public static void AddLoginEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/login", async (LoginRequest model, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtTokenGenerator tokenGenerator) =>
        {
            if ((string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password)) || (model.Username == "string" || model.Password == "string"))
            {
                return Results.BadRequest("Invalid username or password");
            }

            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null || !(await signInManager.CheckPasswordSignInAsync(user, model.Password, false)).Succeeded)
            {
                return Results.Unauthorized();
            }

            var token = tokenGenerator.GenerateToken(user, await userManager.GetRolesAsync(user));

            // You would also generate a refresh token here and save it to the database or a cache

            return Results.Ok(new LoginResponse
            {
                Token = token,
                IsLockedOut = user.LockoutEnabled,
                RequiresTwoFactor = user.TwoFactorEnabled,
                // refreshToken = "Your generated refresh token here"
            });
        })
        .WithName("Login")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status400BadRequest);
    }


    public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder routes, Roles rolesConfig)
    {
        var groups = routes.MapGroup("/auth").WithName(GetAuthEndpointName);

        groups.MapGet("/Health", () => "OK from Auth.Min.API");

        // Add registration and complete profile endpoints
        groups.AddRegistrationEndpoints();
        groups.AddLoginEndpoint();
        groups.AddCompleteProfileEndpoint(rolesConfig);

        return groups;
    }

    private static bool IsRoleValid(string role, Roles rolesConfig)
    {
        var roleProperties = rolesConfig.GetType().GetProperties();
        return roleProperties.Any(prop => (string?)prop.GetValue(rolesConfig) == role);
    }
}
