using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Repositories;
using Microsoft.AspNetCore.Routing;

namespace Frieght.Api.Endpoints;

public static class HealthEndpoints
{
    const string HealthEndpointName = "/";

    public static RouteGroupBuilder MapHealthEndpoints(this IEndpointRouteBuilder routes)
    {

        var groups = routes.MapGroup("/")
            .WithParameterValidation();

         groups.MapGet("/", () => "OK from Freight.API")
        .WithName(HealthEndpointName);

        return groups;
    }
}


