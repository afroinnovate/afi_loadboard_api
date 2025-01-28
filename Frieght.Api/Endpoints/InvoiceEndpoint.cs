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

        group.MapGet("/{id:int}", async (int id,
            IInvoiceRepository repo,
            IPaymentMethodRepository paymentRepo,
            IMapper mapper,
            ILogger<LoggerCategory> logger) =>
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

                // Get associated payment method
                var paymentMethod = await paymentRepo.GetByPaymentMethodIdAsync(invoice.PaymentMethodId);

                // Create response DTO with payment information
                var responseDto = mapper.Map<InvoiceDto>(invoice);
                responseDto.PaymentMethod = mapper.Map<PaymentMethodDto>(paymentMethod);

                logger.LogInformation("Successfully retrieved invoice with ID: {Id}", id);
                return Results.Ok(responseDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve invoice with ID: {Id}", id);
                return Results.Problem("An error occurred while retrieving the invoice", statusCode: 500);
            }
        });

        group.MapPost("/", async (InvoiceDto invoiceDto,
            IInvoiceRepository repo,
            IPaymentMethodRepository paymentRepo,
            IMapper mapper,
            ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Creating new invoice");

                if (invoiceDto.PaymentMethod == null)
                {
                    logger.LogError("Payment method information is required for invoice creation");
                    return Results.BadRequest("Payment method information is required");
                }

                // First check for existing payment method based on payment details
                PaymentMethod? existingPayment = null;
                string paymentMethodId;

                switch (invoiceDto.PaymentMethod.PaymentType)
                {
                    case "Bank" when !string.IsNullOrEmpty(invoiceDto.PaymentMethod.BankAccount):
                        existingPayment = await paymentRepo.GetByBankAccountAsync(invoiceDto.PaymentMethod.BankAccount);
                        break;
                    case "Card" when !string.IsNullOrEmpty(invoiceDto.PaymentMethod.LastFourDigits):
                        existingPayment = await paymentRepo.GetByLastFourDigitsAsync(invoiceDto.PaymentMethod.LastFourDigits);
                        break;
                    case "Mobile" when !string.IsNullOrEmpty(invoiceDto.PaymentMethod.PhoneNumber):
                        existingPayment = await paymentRepo.GetByPhoneNumberAsync(invoiceDto.PaymentMethod.PhoneNumber);
                        break;
                }

                if (existingPayment != null)
                {
                    paymentMethodId = existingPayment.PaymentMethodId;
                    logger.LogInformation("Using existing payment method with ID: {PaymentMethodId}", paymentMethodId);
                }
                else
                {
                    paymentMethodId = $"PA-{invoiceDto.LoadId}-{DateTime.Now:MMdd}";
                    logger.LogInformation("Creating new payment method");
                    var paymentMethod = mapper.Map<PaymentMethod>(invoiceDto.PaymentMethod);
                    paymentMethod.PaymentMethodId = paymentMethodId;
                    paymentMethod.CarrierId = invoiceDto.CarrierId;
                    await paymentRepo.AddAsync(paymentMethod);
                    logger.LogInformation("Successfully created payment method with ID: {PaymentMethodId}", paymentMethodId);
                }

                // Create invoice with payment reference
                var invoice = mapper.Map<Invoice>(invoiceDto);
                invoice.PaymentMethodId = paymentMethodId;
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
