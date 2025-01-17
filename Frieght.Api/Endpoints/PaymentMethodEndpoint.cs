using AutoMapper;
using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Repositories;

namespace Frieght.Api.Endpoints;

public static class PaymentMethodEndpoints
{
    public static void MapPaymentMethodEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/payment-methods");

        group.MapGet("/", async (IPaymentMethodRepository repo) =>
        {
            var paymentMethods = await repo.GetAllAsync();
            return Results.Ok(paymentMethods);
        });

        group.MapGet("/{id:int}", async (int id, IPaymentMethodRepository repo) =>
        {
            var paymentMethod = await repo.GetByIdAsync(id);
            return paymentMethod != null ? Results.Ok(paymentMethod) : Results.NotFound();
        });

        group.MapPost("/", async (PaymentMethodDto paymentMethodDto, IPaymentMethodRepository repo, IMapper mapper) =>
        {
            var paymentMethod = mapper.Map<PaymentMethod>(paymentMethodDto);
            await repo.AddAsync(paymentMethod);
            return Results.Created($"/api/payment-methods/{paymentMethod.Id}", paymentMethod);
        });

        group.MapPut("/{id:int}", async (int id, PaymentMethodDto paymentMethodDto, IPaymentMethodRepository repo, IMapper mapper) =>
        {
            var existingPaymentMethod = await repo.GetByIdAsync(id);
            if (existingPaymentMethod == null) return Results.NotFound();

            mapper.Map(paymentMethodDto, existingPaymentMethod);
            await repo.UpdateAsync(existingPaymentMethod);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, IPaymentMethodRepository repo) =>
        {
            var paymentMethod = await repo.GetByIdAsync(id);
            if (paymentMethod == null) return Results.NotFound();

            await repo.DeleteAsync(paymentMethod.Id);
            return Results.NoContent();
        });
    }
}

