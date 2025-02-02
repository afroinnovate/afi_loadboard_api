using AutoMapper;
using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Repositories;
using Microsoft.Extensions.Logging;

namespace Frieght.Api.Endpoints;

public static class PaymentMethodEndpoints
{
    public class LoggerCategory
    {
        // This class is used to define the category for the logger
    }

    public static RouteGroupBuilder MapPaymentMethodEndpoints(this IEndpointRouteBuilder routes, IMapper mapper)
    {
        var group = routes.MapGroup("/api/payment-methods")
            .WithParameterValidation();

        group.MapGet("/", async (IPaymentMethodRepository repo, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Retrieving all payment methods");
                var paymentMethods = await repo.GetAllAsync();
                logger.LogInformation("Successfully retrieved {Count} payment methods", paymentMethods.Count());
                return Results.Ok(paymentMethods);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve payment methods");
                return Results.Problem("An error occurred while retrieving payment methods", statusCode: 500);
            }
        });

        group.MapGet("/{id:int}", async (int id, IPaymentMethodRepository repo, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Retrieving payment method with ID: {Id}", id);
                var paymentMethod = await repo.GetByIdAsync(id);
                if (paymentMethod == null)
                {
                    logger.LogWarning("Payment method with ID: {Id} not found", id);
                    return Results.NotFound();
                }
                logger.LogInformation("Successfully retrieved payment method with ID: {Id}", id);
                return Results.Ok(paymentMethod);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve payment method with ID: {Id}", id);
                return Results.Problem("An error occurred while retrieving the payment method", statusCode: 500);
            }
        });

        group.MapPost("/", async (PaymentMethodDto paymentMethodDto, IPaymentMethodRepository repo, IMapper mapper, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Creating new payment method");
                var paymentMethod = mapper.Map<PaymentMethod>(paymentMethodDto);
                await repo.AddAsync(paymentMethod);
                logger.LogInformation("Successfully created payment method with ID: {Id}", paymentMethod.Id);
                return Results.Created($"/api/payment-methods/{paymentMethod.Id}", paymentMethod);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to create payment method");
                return Results.Problem("An error occurred while creating the payment method", statusCode: 500);
            }
        });

        group.MapPut("/{id:int}", async (int id, PaymentMethodDto paymentMethodDto, IPaymentMethodRepository repo, IMapper mapper, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Updating payment method with ID: {Id}", id);
                var existingPaymentMethod = await repo.GetByIdAsync(id);
                if (existingPaymentMethod == null)
                {
                    logger.LogWarning("Payment method with ID: {Id} not found", id);
                    return Results.NotFound();
                }

                mapper.Map(paymentMethodDto, existingPaymentMethod);
                await repo.UpdateAsync(existingPaymentMethod);
                logger.LogInformation("Successfully updated payment method with ID: {Id}", id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update payment method with ID: {Id}", id);
                return Results.Problem("An error occurred while updating the payment method", statusCode: 500);
            }
        });

        group.MapDelete("/{id:int}", async (int id, IPaymentMethodRepository repo, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Deleting payment method with ID: {Id}", id);
                var paymentMethod = await repo.GetByIdAsync(id);
                if (paymentMethod == null)
                {
                    logger.LogWarning("Payment method with ID: {Id} not found", id);
                    return Results.NotFound();
                }

                await repo.DeleteAsync(paymentMethod.Id);
                logger.LogInformation("Successfully deleted payment method with ID: {Id}", id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete payment method with ID: {Id}", id);
                return Results.Problem("An error occurred while deleting the payment method", statusCode: 500);
            }
        });

        group.MapGet("/by-payment-id/{paymentMethodId}", async (string paymentMethodId,
            IPaymentMethodRepository repo,
            ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Retrieving payment method with payment ID: {PaymentMethodId}", paymentMethodId);
                var paymentMethod = await repo.GetByPaymentMethodIdAsync(paymentMethodId);
                if (paymentMethod == null)
                {
                    logger.LogWarning("Payment method with payment ID: {PaymentMethodId} not found", paymentMethodId);
                    return Results.NotFound();
                }
                logger.LogInformation("Successfully retrieved payment method with payment ID: {PaymentMethodId}", paymentMethodId);
                return Results.Ok(paymentMethod);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve payment method with payment ID: {PaymentMethodId}", paymentMethodId);
                return Results.Problem("An error occurred while retrieving the payment method", statusCode: 500);
            }
        });

        return group;
    }
}

