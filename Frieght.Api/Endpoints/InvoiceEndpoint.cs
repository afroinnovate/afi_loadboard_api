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
            [FromServices] IExternalUserService userService,
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

                // Fetch carrier details from external service
                var carrier = await userService.GetUserAsync(invoiceDto.CarrierId);
                if (carrier == null)
                {
                    logger.LogError("Carrier not found with ID: {CarrierId}", invoiceDto.CarrierId);
                    return Results.BadRequest("Invalid carrier ID");
                }

                // Store carrier details in the invoice
                invoiceDto.CarrierName = carrier.FullName;
                invoiceDto.CarrierEmail = carrier.Email;
                invoiceDto.CarrierPhone = carrier.Phone;
                invoiceDto.CarrierBusinessName = carrier.Company;

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

                // Create invoice with carrier details
                var invoice = mapper.Map<Invoice>(invoiceDto);
                invoice.PaymentMethodId = paymentMethodId;
                // Convert dates to UTC
                invoice.IssueDate = invoice.IssueDate.ToUniversalTime();
                invoice.DueDate = invoice.DueDate.ToUniversalTime();
                if (invoice.TransactionDate.HasValue)
                {
                    invoice.TransactionDate = invoice.TransactionDate.Value.ToUniversalTime();
                }
                await repo.AddAsync(invoice);

                // For response, include the full carrier object
                var responseDto = mapper.Map<InvoiceDto>(invoice);
                responseDto.Carrier = carrier;

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

                hasChanges |= existingInvoice.Status.ToLower() != invoiceDto.Status.ToLower();
                hasChanges |= existingInvoice.Note != invoiceDto.Note;
                hasChanges |= existingInvoice.TransactionStatus != invoiceDto.TransactionStatus;
                hasChanges |= existingInvoice.DueDate.Date != invoiceDto.DueDate.Date;
                hasChanges |= existingInvoice.AmountDue != invoiceDto.AmountDue;
                hasChanges |= existingInvoice.TotalAmount != invoiceDto.TotalAmount;
                hasChanges |= existingInvoice.TotalVat != invoiceDto.TotalVat;
                hasChanges |= existingInvoice.Withholding != invoiceDto.Withholding;
                hasChanges |= existingInvoice.ServiceFees != invoiceDto.ServiceFees;
                hasChanges |= existingInvoice.TransactionId != invoiceDto.TransactionId;

                // Check TransactionDate separately due to nullable DateTime
                if (!string.IsNullOrEmpty(invoiceDto.TransactionDate))
                {
                    if (DateTime.TryParse(invoiceDto.TransactionDate, out DateTime parsedDate))
                    {
                        hasChanges |= existingInvoice.TransactionDate?.Date != parsedDate.Date;
                    }
                }
                else
                {
                    hasChanges |= existingInvoice.TransactionDate != null;
                }

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
                existingInvoice.Status = invoiceDto.Status.ToLower();
                existingInvoice.Note = invoiceDto.Note;
                existingInvoice.TransactionStatus = invoiceDto.TransactionStatus;
                existingInvoice.DueDate = invoiceDto.DueDate.ToUniversalTime();
                existingInvoice.AmountDue = invoiceDto.AmountDue;
                existingInvoice.TotalAmount = invoiceDto.TotalAmount;
                existingInvoice.TotalVat = invoiceDto.TotalVat;
                existingInvoice.Withholding = invoiceDto.Withholding;
                existingInvoice.ServiceFees = invoiceDto.ServiceFees;
                existingInvoice.TransactionId = invoiceDto.TransactionId;

                // Parse the transaction date if it's provided
                if (!string.IsNullOrEmpty(invoiceDto.TransactionDate))
                {
                    if (DateTime.TryParse(invoiceDto.TransactionDate, out DateTime parsedDate))
                    {
                        existingInvoice.TransactionDate = parsedDate.ToUniversalTime();
                    }
                    else
                    {
                        logger.LogWarning("Invalid transaction date format provided for invoice ID: {Id}", id);
                        return Results.BadRequest("Invalid transaction date format");
                    }
                }
                else
                {
                    existingInvoice.TransactionDate = null;
                }

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
