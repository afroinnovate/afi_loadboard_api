using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Repositories;
using Microsoft.AspNetCore.Routing;

namespace Frieght.Api.Endpoints;

public static class CarrierEndpoints
{
    const string GetCarrierEndpointName = "GetCarrier";

    public static RouteGroupBuilder MapCarriersEndpoints(this IEndpointRouteBuilder routes)
    {

        var groups = routes.MapGroup("/carriers")
            .WithParameterValidation();

        groups.MapGet("/", async (ICarrierRepository repository) => (await repository.GetCarriers()).Select(carrier => carrier.asDto()));
        groups.MapGet("/{id}", async (ICarrierRepository repository, string id) =>
        {
            User? carrier = await repository.GetCarrier(id);
            return carrier is not null ? Results.Ok(carrier.asDto()) : Results.NotFound();



        }).WithName(GetCarrierEndpointName);

        groups.MapPost("/", async (ICarrierRepository repository, CreateCarrierDto carrierDto) =>
        {
            User carrier = new()
            {
                UserId = carrierDto.UserId,
                FirstName = carrierDto.FirstName,
                LastName = carrierDto.LastName,
                CompanyName = carrierDto.CompanyName,
                Email = carrierDto.Email,
                Phone = carrierDto.Phone,
                MotorCarrierNumber = carrierDto.MotorCarrierNumber,
                DOTNumber = carrierDto.DOTNumber,
                EquipmentType = carrierDto.EquipmentType,
                AvailableCapacity = carrierDto.AvailableCapacity,
            };
            await repository.CreateCarrier(carrier);
            return Results.CreatedAtRoute(GetCarrierEndpointName, new { id = carrier.UserId }, carrier);

        });

        groups.MapPut("/{id}", async (ICarrierRepository repository, string id, UpdateCarrierDto updatedCarrierDto) =>
        {
            User? existingCarrier = await repository.GetCarrier(id);
            if (existingCarrier is null) return Results.NotFound();
     
            existingCarrier.UserId = updatedCarrierDto.UserId;
            existingCarrier.CompanyName = updatedCarrierDto.CompanyName;    
            existingCarrier.Phone   = updatedCarrierDto.Phone;
            existingCarrier.Email = updatedCarrierDto.Email;
            existingCarrier.MotorCarrierNumber = updatedCarrierDto.MotorCarrierNumber;
            existingCarrier.DOTNumber = updatedCarrierDto.DOTNumber;
            existingCarrier.EquipmentType = updatedCarrierDto.EquipmentType;
            existingCarrier.AvailableCapacity = updatedCarrierDto.AvailableCapacity;

            await repository.UpdateCarrier(existingCarrier);

            return Results.NoContent();

        });

        groups.MapDelete("/{id}", async (ICarrierRepository repository, string id) =>
        {
            User? carrier = await repository.GetCarrier(id);
            if (carrier is not null) await repository.DeleteCarrier(carrier);

            return Results.NoContent();

        });

        return groups;


    }
}


