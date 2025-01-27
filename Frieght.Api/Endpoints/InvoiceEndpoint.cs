using AutoMapper;
using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Repositories;
using Frieght.Api.Services;

namespace Frieght.Api.Endpoints;

public static class InvoiceEndpoints
{
    public class LoggerCategory
    {
        // This class is used to define the category for the logger
    }

    public static RouteGroupBuilder MapInvoiceEndpoints(this IEndpointRouteBuilder routes, IMapper mapper)
    {
        var group = routes.MapGroup("/api/invoices")
            .WithParameterValidation();

        group.MapGet("/", async (IInvoiceRepository repo, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Retrieving all invoices");
                var invoices = await repo.GetAllAsync();
                logger.LogInformation("Successfully retrieved {Count} invoices", invoices.Count());
                return Results.Ok(invoices);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve all invoices");
                return Results.Problem("An error occurred while retrieving invoices", statusCode: 500);
            }
        });

        group.MapGet("/{id:int}", async (int id, IInvoiceRepository repo, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Retrieving invoice with ID: {Id}", id);
                var invoice = await repo.GetByIdAsync(id);
                if (invoice == null)
                {
                    logger.LogWarning("Invoice with ID: {Id} not found", id);
                    return Results.NotFound();
                }
                logger.LogInformation("Successfully retrieved invoice with ID: {Id}", id);
                return Results.Ok(invoice);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve invoice with ID: {Id}", id);
                return Results.Problem("An error occurred while retrieving the invoice", statusCode: 500);
            }
        });

        group.MapPost("/", async (InvoiceDto invoiceDto, IInvoiceRepository repo, IMapper mapper, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Creating new invoice");
                var invoice = mapper.Map<Invoice>(invoiceDto);
                await repo.AddAsync(invoice);
                logger.LogInformation("Successfully created invoice with ID: {Id}", invoice.Id);
                return Results.Created($"/api/invoices/{invoice.Id}", invoice);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to create invoice");
                return Results.Problem("An error occurred while creating the invoice", statusCode: 500);
            }
        });

        group.MapPut("/{id:int}", async (int id, InvoiceDto invoiceDto, IInvoiceRepository repo, IMapper mapper, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Updating invoice with ID: {Id}", id);
                var existingInvoice = await repo.GetByIdAsync(id);
                if (existingInvoice == null)
                {
                    logger.LogWarning("Invoice with ID: {Id} not found", id);
                    return Results.NotFound();
                }

                mapper.Map(invoiceDto, existingInvoice);
                await repo.UpdateAsync(existingInvoice);
                logger.LogInformation("Successfully updated invoice with ID: {Id}", id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update invoice with ID: {Id}", id);
                return Results.Problem("An error occurred while updating the invoice", statusCode: 500);
            }
        });

        group.MapDelete("/{id:int}", async (int id, IInvoiceRepository repo, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Deleting invoice with ID: {Id}", id);
                var invoice = await repo.GetByIdAsync(id);
                if (invoice == null)
                {
                    logger.LogWarning("Invoice with ID: {Id} not found", id);
                    return Results.NotFound();
                }

                await repo.DeleteAsync(id);
                logger.LogInformation("Successfully deleted invoice with ID: {Id}", id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete invoice with ID: {Id}", id);
                return Results.Problem("An error occurred while deleting the invoice", statusCode: 500);
            }
        });

        group.MapGet("/carrier/{carrierId}", async (string carrierId, IInvoiceService service, ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Retrieving invoices for carrier with ID: {CarrierId}", carrierId);
                var invoices = await service.GetByCarrierIdAsync(carrierId);
                if (!invoices.Any())
                {
                    logger.LogWarning("No invoices found for carrier with ID: {CarrierId}", carrierId);
                    return Results.NotFound();
                }
                logger.LogInformation("Successfully retrieved {Count} invoices for carrier with ID: {CarrierId}", invoices.Count(), carrierId);
                return Results.Ok(invoices);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve invoices for carrier with ID: {CarrierId}", carrierId);
                return Results.Problem("An error occurred while retrieving the invoices", statusCode: 500);
            }
        });

        group.MapGet("/number/{invoiceNumber}", async (string invoiceNumber, IInvoiceService service) =>
        {
            var invoice = await service.GetByInvoiceNumberAsync(invoiceNumber);
            return invoice != null ? Results.Ok(invoice) : Results.NotFound();
        });

        group.MapGet("/load/{loadId:int}", async (int loadId, IInvoiceService service) =>
        {
            var invoice = await service.GetByLoadIdAsync(loadId);
            return invoice != null ? Results.Ok(invoice) : Results.NotFound();
        });

        return group;
    }
}
