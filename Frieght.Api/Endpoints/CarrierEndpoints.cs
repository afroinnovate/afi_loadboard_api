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
        groups.MapGet("/{id}", async (ICarrierRepository repository, int id) =>
        {
            Carrier? carrier = await repository.GetCarrier(id);
            return carrier is not null ? Results.Ok(carrier.asDto()) : Results.NotFound();



        }).WithName(GetCarrierEndpointName);

        groups.MapPost("/", async (ICarrierRepository repository, CreateCarrierDto carrierDto) =>
        {
            Carrier carrier = new()
            {
                UserId = carrierDto.UserId,
                CompanyName = carrierDto.CompanyName,
                CompanyEmail = carrierDto.CompanyEmail,
                CompanyPhone = carrierDto.CompanyPhone,
                MotorCarrierNumber = carrierDto.MotorCarrierNumber,
                DOTNumber = carrierDto.DOTNumber,
                EquipmentType = carrierDto.EquipmentType,
                AvailableCapacity = carrierDto.AvailableCapacity,
            };
            await repository.CreateCarrier(carrier);
            return Results.CreatedAtRoute(GetCarrierEndpointName, new { id = carrier.Id }, carrier);

        });

        groups.MapPut("/{id}", async (ICarrierRepository repository, int id, UpdateCarrierDto updatedCarrierDto) =>
        {
            Carrier? existingCarrier = await repository.GetCarrier(id);
            if (existingCarrier is null) return Results.NotFound();
     
            existingCarrier.UserId = updatedCarrierDto.UserId;
            existingCarrier.CompanyName = updatedCarrierDto.CompanyName;    
            existingCarrier.CompanyPhone   = updatedCarrierDto.CompanyPhone;
            existingCarrier.CompanyEmail = updatedCarrierDto.CompanyEmail;
            existingCarrier.MotorCarrierNumber = updatedCarrierDto.MotorCarrierNumber;
            existingCarrier.DOTNumber = updatedCarrierDto.DOTNumber;
            existingCarrier.EquipmentType = updatedCarrierDto.EquipmentType;
            existingCarrier.AvailableCapacity = updatedCarrierDto.AvailableCapacity;

            await repository.UpdateCarrier(existingCarrier);

            return Results.NoContent();

        });

        groups.MapDelete("/{id}", async (ICarrierRepository repository, int id) =>
        {
            Carrier? carrier = await repository.GetCarrier(id);
            if (carrier is not null) await repository.DeleteCarrier(carrier);

            return Results.NoContent();

        });

        return groups;


    }
}


