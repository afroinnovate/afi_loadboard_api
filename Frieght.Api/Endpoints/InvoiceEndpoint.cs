using AutoMapper;
using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Repositories;
using Frieght.Api.Services;
using Microsoft.AspNetCore.Mvc;

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

        group.MapGet("/", async (
            [FromServices] IInvoiceRepository repo,
            [FromServices] IPaymentMethodRepository paymentRepo,
            [FromServices] IMapper mapper,
            [FromServices] ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Retrieving all invoices");
                var invoices = await repo.GetAllAsync();

                // Create response DTOs with payment information
                var responseDtos = new List<InvoiceDto>();
                foreach (var invoice in invoices)
                {
                    var dto = mapper.Map<InvoiceDto>(invoice);
                    if (!string.IsNullOrEmpty(invoice.PaymentMethodId))
                    {
                        var payment = await paymentRepo.GetByPaymentMethodIdAsync(invoice.PaymentMethodId);
                        dto.PaymentMethod = mapper.Map<PaymentMethodDto>(payment);
                    }
                    responseDtos.Add(dto);
                }

                logger.LogInformation("Successfully retrieved {Count} invoices", invoices.Count());
                return Results.Ok(responseDtos);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve all invoices");
                return Results.Problem("An error occurred while retrieving invoices", statusCode: 500);
            }
        });

        group.MapGet("/{id:int}", async (
            int id,
            [FromServices] IInvoiceRepository repo,
            [FromServices] IPaymentMethodRepository paymentRepo,
            [FromServices] IMapper mapper,
            [FromServices] ILogger<LoggerCategory> logger) =>
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

                // Create response DTO
                var responseDto = mapper.Map<InvoiceDto>(invoice);

                // Get associated payment method if exists
                if (!string.IsNullOrEmpty(invoice.PaymentMethodId))
                {
                    var paymentMethod = await paymentRepo.GetByPaymentMethodIdAsync(invoice.PaymentMethodId);
                    if (paymentMethod != null)
                    {
                        responseDto.PaymentMethod = mapper.Map<PaymentMethodDto>(paymentMethod);
                    }
                }

                logger.LogInformation("Successfully retrieved invoice with ID: {Id}", id);
                return Results.Ok(responseDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve invoice with ID: {Id}", id);
                return Results.Problem("An error occurred while retrieving the invoice", statusCode: 500);
            }
        });

        group.MapPost("/", async (
            InvoiceDto invoiceDto,
            [FromServices] IInvoiceRepository repo,
            [FromServices] IPaymentMethodRepository paymentRepo,
            [FromServices] IMapper mapper,
            [FromServices] ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Creating new invoice");

                if (invoiceDto.PaymentMethod == null)
                {
                    logger.LogError("Payment method information is required for invoice creation");
                    return Results.BadRequest("Payment method information is required");
                }

                // Validate payment method details based on type
                string paymentMethodId;
                PaymentMethod? existingPayment = null;

                switch (invoiceDto.PaymentMethod.PaymentType.ToLower())
                {
                    case "mobile_money":
                        if (string.IsNullOrEmpty(invoiceDto.PaymentMethod.PhoneNumber))
                        {
                            logger.LogError("Phone number is required for mobile money payment method");
                            return Results.BadRequest("Phone number is required for mobile money payment");
                        }
                        paymentMethodId = $"PA-{invoiceDto.LoadId}-{invoiceDto.PaymentMethod.PhoneNumber}";
                        existingPayment = await paymentRepo.GetByPaymentMethodIdAsync(paymentMethodId);
                        break;

                    case "bank":
                        if (string.IsNullOrEmpty(invoiceDto.PaymentMethod.BankAccount))
                        {
                            logger.LogError("Bank account is required for bank payment method");
                            return Results.BadRequest("Bank account is required for bank payment");
                        }
                        var lastFourOfBank = invoiceDto.PaymentMethod.BankAccount.Length >= 4
                            ? invoiceDto.PaymentMethod.BankAccount[^4..]
                            : invoiceDto.PaymentMethod.BankAccount;
                        paymentMethodId = $"PA-{invoiceDto.LoadId}-{lastFourOfBank}";
                        existingPayment = await paymentRepo.GetByPaymentMethodIdAsync(paymentMethodId);
                        break;

                    case "card":
                        if (string.IsNullOrEmpty(invoiceDto.PaymentMethod.LastFourDigits))
                        {
                            logger.LogError("Card last four digits are required for card payment method");
                            return Results.BadRequest("Card last four digits are required for card payment");
                        }
                        paymentMethodId = $"PA-{invoiceDto.LoadId}-{invoiceDto.PaymentMethod.LastFourDigits}";
                        existingPayment = await paymentRepo.GetByPaymentMethodIdAsync(paymentMethodId);
                        break;

                    default:
                        logger.LogError("Invalid payment type: {PaymentType}", invoiceDto.PaymentMethod.PaymentType);
                        return Results.BadRequest($"Invalid payment type: {invoiceDto.PaymentMethod.PaymentType}");
                }

                if (existingPayment != null)
                {
                    logger.LogInformation("Using existing payment method with ID: {PaymentMethodId}", paymentMethodId);
                }
                else
                {
                    logger.LogInformation("Creating new payment method with ID: {PaymentMethodId}", paymentMethodId);
                    var paymentMethod = mapper.Map<PaymentMethod>(invoiceDto.PaymentMethod);
                    paymentMethod.PaymentMethodId = paymentMethodId;
                    paymentMethod.CarrierId = invoiceDto.CarrierId;
                    await paymentRepo.AddAsync(paymentMethod);
                    logger.LogInformation("Successfully created payment method with ID: {PaymentMethodId}", paymentMethodId);
                }

                // Create invoice with carrier details from DTO
                var invoice = mapper.Map<Invoice>(invoiceDto);
                invoice.PaymentMethodId = paymentMethodId;
                // Convert dates to UTC
                invoice.IssueDate = invoice.IssueDate.ToUniversalTime();
                invoice.DueDate = invoice.DueDate.ToUniversalTime();
                if (invoice.TransactionDate.HasValue)
                {
                    invoice.TransactionDate = invoice.TransactionDate.Value.ToUniversalTime();
                }

                // The ID will be auto-incrementing as it's handled by the database
                await repo.AddAsync(invoice);

                var responseDto = mapper.Map<InvoiceDto>(invoice);

                logger.LogInformation("Successfully created invoice with ID: {Id}", invoice.Id);
                return Results.Created($"/api/invoices/{invoice.Id}", responseDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to create invoice");
                return Results.Problem("An error occurred while creating the invoice", statusCode: 500);
            }
        });

        group.MapPut("/{id:int}", async (
            int id,
            InvoiceDto invoiceDto,
            [FromServices] IInvoiceRepository repo,
            [FromServices] IMapper mapper,
            [FromServices] ILogger<LoggerCategory> logger) =>
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

                // Validate immutable fields
                if (existingInvoice.PaymentMethodId != invoiceDto.PaymentMethodId)
                {
                    logger.LogWarning("Attempt to modify payment method ID from {ExistingId} to {NewId} for invoice ID: {Id}",
                        existingInvoice.PaymentMethodId, invoiceDto.PaymentMethodId, id);
                    return Results.BadRequest("Payment method ID cannot be modified");
                }

                if (existingInvoice.CarrierId != invoiceDto.CarrierId)
                {
                    logger.LogWarning("Attempt to modify carrier ID from {ExistingId} to {NewId} for invoice ID: {Id}",
                        existingInvoice.CarrierId, invoiceDto.CarrierId, id);
                    return Results.BadRequest("Carrier ID cannot be modified");
                }

                // Check if any mutable fields have changed
                bool hasChanges = false;

                hasChanges |= !string.IsNullOrEmpty(invoiceDto.Status) &&
                    existingInvoice.Status.ToLower() != invoiceDto.Status.ToLower();
                hasChanges |= !string.IsNullOrEmpty(invoiceDto.Note) &&
                    existingInvoice.Note != invoiceDto.Note;
                hasChanges |= !string.IsNullOrEmpty(invoiceDto.TransactionStatus) &&
                    existingInvoice.TransactionStatus != invoiceDto.TransactionStatus;
                hasChanges |= !string.IsNullOrEmpty(invoiceDto.DueDate) &&
                    existingInvoice.DueDate.Date != DateTime.Parse(invoiceDto.DueDate).Date;
                hasChanges |= invoiceDto.AmountDue != 0 &&
                    existingInvoice.AmountDue != invoiceDto.AmountDue;
                hasChanges |= invoiceDto.TotalAmount != 0 &&
                    existingInvoice.TotalAmount != invoiceDto.TotalAmount;
                hasChanges |= invoiceDto.TotalVat != 0 &&
                    existingInvoice.TotalVat != invoiceDto.TotalVat;
                hasChanges |= invoiceDto.Withholding != 0 &&
                    existingInvoice.Withholding != invoiceDto.Withholding;
                hasChanges |= invoiceDto.ServiceFees != 0 &&
                    existingInvoice.ServiceFees != invoiceDto.ServiceFees;
                hasChanges |= !string.IsNullOrEmpty(invoiceDto.TransactionId) &&
                    existingInvoice.TransactionId != invoiceDto.TransactionId;
                hasChanges |= !string.IsNullOrEmpty(invoiceDto.TransactionDate) &&
                    existingInvoice.TransactionDate?.Date != DateTime.Parse(invoiceDto.TransactionDate).Date;

                if (!hasChanges)
                {
                    logger.LogInformation("No changes detected for invoice ID: {Id}", id);
                    return Results.BadRequest("No changes detected in the update request");
                }

                // Validate status transitions
                if (!IsValidStatusTransition(existingInvoice.Status, invoiceDto.Status))
                {
                    var normalizedCurrentStatus = existingInvoice.Status.ToLower();
                    var normalizedNewStatus = invoiceDto.Status.ToLower();
                    logger.LogWarning("Invalid status transition from {CurrentStatus} to {NewStatus} for invoice ID: {Id}",
                        normalizedCurrentStatus, normalizedNewStatus, id);
                    return Results.BadRequest($"Invalid status transition from {normalizedCurrentStatus} to {normalizedNewStatus}");
                }

                // Update all mutable fields
                existingInvoice.Status = !string.IsNullOrEmpty(invoiceDto.Status)
                    ? invoiceDto.Status.ToLower()
                    : existingInvoice.Status;

                existingInvoice.Note = !string.IsNullOrEmpty(invoiceDto.Note)
                    ? invoiceDto.Note
                    : existingInvoice.Note;

                existingInvoice.TransactionStatus = !string.IsNullOrEmpty(invoiceDto.TransactionStatus)
                    ? invoiceDto.TransactionStatus
                    : existingInvoice.TransactionStatus;

                // Handle DueDate - keep existing if new is empty
                if (!string.IsNullOrEmpty(invoiceDto.DueDate))
                {
                    existingInvoice.DueDate = DateTime.Parse(invoiceDto.DueDate).ToUniversalTime();
                }

                // Handle decimal values - only update if not 0
                existingInvoice.AmountDue = invoiceDto.AmountDue != 0 ? invoiceDto.AmountDue : existingInvoice.AmountDue;
                existingInvoice.TotalAmount = invoiceDto.TotalAmount != 0 ? invoiceDto.TotalAmount : existingInvoice.TotalAmount;
                existingInvoice.TotalVat = invoiceDto.TotalVat != 0 ? invoiceDto.TotalVat : existingInvoice.TotalVat;
                existingInvoice.Withholding = invoiceDto.Withholding != 0 ? invoiceDto.Withholding : existingInvoice.Withholding;
                existingInvoice.ServiceFees = invoiceDto.ServiceFees != 0 ? invoiceDto.ServiceFees : existingInvoice.ServiceFees;

                existingInvoice.TransactionId = !string.IsNullOrEmpty(invoiceDto.TransactionId)
                    ? invoiceDto.TransactionId
                    : existingInvoice.TransactionId;

                // Handle TransactionDate - only update if new value provided
                if (!string.IsNullOrEmpty(invoiceDto.TransactionDate))
                {
                    existingInvoice.TransactionDate = DateTime.Parse(invoiceDto.TransactionDate).ToUniversalTime();
                }
                // Don't set to null unless explicitly requested

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

        group.MapGet("/carrier/{carrierId}", async (
            string carrierId,
            [FromServices] IInvoiceService service,
            [FromServices] IPaymentMethodRepository paymentRepo,
            [FromServices] IMapper mapper,
            [FromServices] ILogger<LoggerCategory> logger) =>
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

                // Add payment information to each invoice
                foreach (var invoice in invoices)
                {
                    if (!string.IsNullOrEmpty(invoice.PaymentMethodId))
                    {
                        var payment = await paymentRepo.GetByPaymentMethodIdAsync(invoice.PaymentMethodId);
                        invoice.PaymentMethod = mapper.Map<PaymentMethodDto>(payment);
                    }
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

        group.MapGet("/number/{invoiceNumber}", async (
            string invoiceNumber, 
            [FromServices] IInvoiceService service,
            [FromServices] ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Retrieving invoice with number: {InvoiceNumber}", invoiceNumber);
                var invoice = await service.GetByInvoiceNumberAsync(invoiceNumber);
                if (invoice == null)
                {
                    logger.LogWarning("Invoice with number {InvoiceNumber} not found", invoiceNumber);
                    return Results.NotFound();
                }
                
                logger.LogInformation("Successfully retrieved invoice with number: {InvoiceNumber}", invoiceNumber);
                return Results.Ok(invoice);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve invoice with number: {InvoiceNumber}", invoiceNumber);
                return Results.Problem("An error occurred while retrieving the invoice", statusCode: 500);
            }
        });

        group.MapGet("/load/{loadId:int}", async (
            int loadId, 
            [FromServices] IInvoiceService service,
            [FromServices] ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Retrieving invoice for load ID: {LoadId}", loadId);
                var invoice = await service.GetByLoadIdAsync(loadId);
                if (invoice == null)
                {
                    logger.LogWarning("Invoice for load ID {LoadId} not found", loadId);
                    return Results.NotFound();
                }
                
                logger.LogInformation("Successfully retrieved invoice for load ID: {LoadId}", loadId);
                return Results.Ok(invoice);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve invoice for load ID: {LoadId}", loadId);
                return Results.Problem("An error occurred while retrieving the invoice", statusCode: 500);
            }
        });

        group.MapGet("/shipper/{shipperId}", async (
            string shipperId,
            [FromServices] IInvoiceRepository repo,
            [FromServices] IPaymentMethodRepository paymentRepo,
            [FromServices] IMapper mapper,
            [FromServices] ILogger<LoggerCategory> logger) =>
        {
            try
            {
                logger.LogInformation("Retrieving invoices for shipper with ID: {ShipperId}", shipperId);
                var invoices = await repo.GetByShipperIdAsync(shipperId);

                if (!invoices.Any())
                {
                    logger.LogWarning("No invoices found for shipper with ID: {ShipperId}", shipperId);
                    return Results.NotFound();
                }

                // Map to DTOs and include payment information
                var invoiceDtos = new List<InvoiceDto>();
                foreach (var invoice in invoices)
                {
                    var dto = mapper.Map<InvoiceDto>(invoice);
                    if (!string.IsNullOrEmpty(invoice.PaymentMethodId))
                    {
                        var payment = await paymentRepo.GetByPaymentMethodIdAsync(invoice.PaymentMethodId);
                        dto.PaymentMethod = mapper.Map<PaymentMethodDto>(payment);
                    }
                    invoiceDtos.Add(dto);
                }

                logger.LogInformation("Successfully retrieved {Count} invoices for shipper with ID: {ShipperId}",
                    invoices.Count(), shipperId);
                return Results.Ok(invoiceDtos);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve invoices for shipper with ID: {ShipperId}", shipperId);
                return Results.Problem("An error occurred while retrieving the invoices", statusCode: 500);
            }
        });

        return group;
    }

    // Helper method to validate status transitions
    private static bool IsValidStatusTransition(string currentStatus, string newStatus)
    {
        // Normalize status strings to handle case-insensitive comparison
        currentStatus = (currentStatus ?? "").ToLower().Trim();
        newStatus = (newStatus ?? "").ToLower().Trim();

        // Define valid status transitions (all lowercase)
        var validTransitions = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        // Initialize the transitions with proper HashSet initialization
        validTransitions["pending"] = new HashSet<string>(new[]
            { "processing", "cancelled", "paid", "completed" },
            StringComparer.OrdinalIgnoreCase);

        validTransitions["processing"] = new HashSet<string>(new[]
            { "completed", "failed", "paid" },
            StringComparer.OrdinalIgnoreCase);

        validTransitions["failed"] = new HashSet<string>(new[]
            { "processing" },
            StringComparer.OrdinalIgnoreCase);

        validTransitions["completed"] = new HashSet<string>(Array.Empty<string>(),
            StringComparer.OrdinalIgnoreCase);

        validTransitions["cancelled"] = new HashSet<string>(Array.Empty<string>(),
            StringComparer.OrdinalIgnoreCase);

        validTransitions["paid"] = new HashSet<string>(Array.Empty<string>(),
            StringComparer.OrdinalIgnoreCase);

        // Check if the current status has any valid transitions
        if (!validTransitions.ContainsKey(currentStatus))
        {
            return false;
        }

        // Check if the new status is a valid transition from the current status
        return validTransitions[currentStatus].Contains(newStatus);
    }
}
