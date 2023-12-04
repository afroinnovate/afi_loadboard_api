
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Auth.Min.API.Endpoints;
public static class AuthEndpoints
{
    const string GetAuthEndpointName = "auth";

    public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {

        var groups = routes.MapGroup("/").WithName(GetAuthEndpointName);

        groups.MapGet("/Health", () => "OK from Auth.Min.API");

       return groups;
    }
}
