
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Auth.Min.API.Endpoints;
public static class AuthEndpoints
{
    const string GetAuthEndpointName = "auth";

    public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {

        var groups = routes.MapGroup("/").WithName(GetAuthEndpointName);

        groups.MapPost("/login", (string username) =>
        {
            var claimsPrincipal = new ClaimsPrincipal(
            new ClaimsIdentity(
                new Claim[] {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                },
                BearerTokenDefaults.AuthenticationScheme  //👈
            ));

            return Results.SignIn(claimsPrincipal);
        });

       return groups;
    }
}
