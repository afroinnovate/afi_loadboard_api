using Frieght.Api.Entities;
using Frieght.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Frieght.Api.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly FrieghtDbContext _context;
    private readonly IPaymentMethodRepository _paymentRepo;
    private readonly ILogger<InvoiceRepository> _logger;

    public InvoiceRepository(
        FrieghtDbContext context,
        IPaymentMethodRepository paymentRepo,
        ILogger<InvoiceRepository> logger)
    {
        _context = context;
        _paymentRepo = paymentRepo;
        _logger = logger;
    }

    public async Task<IEnumerable<Invoice>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all invoices");
            var invoices = await _context.Invoices.ToListAsync();
            _logger.LogInformation("Successfully retrieved {Count} invoices", invoices.Count);
            return invoices;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching all invoices");
            throw;
        }
    }

    public async Task<Invoice?> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Fetching invoice with ID: {Id}", id);
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                _logger.LogWarning("Invoice with ID: {Id} not found", id);
            }
            else
            {
                _logger.LogInformation("Successfully retrieved invoice with ID: {Id}", id);
            }

            return invoice;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching invoice with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Invoice>> GetByCarrierIdAsync(string carrierId)
    {
        try
        {
            _logger.LogInformation("Fetching invoices for carrier ID: {CarrierId}", carrierId);
            var invoices = await _context.Invoices
                .Where(i => i.CarrierId == carrierId)
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} invoices for carrier ID: {CarrierId}",
                invoices.Count, carrierId);
            return invoices;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching invoices for carrier ID: {CarrierId}", carrierId);
            throw;
        }
    }

    public async Task UpdateAsync(Invoice invoice)
    {
        try
        {
            _logger.LogInformation("Starting update for invoice ID: {Id}", invoice.Id);

            // Log the current state of the invoice
            _logger.LogDebug("Current invoice state - Status: {Status}, TransactionStatus: {TransactionStatus}, " +
                           "TransactionDate: {TransactionDate}, Note: {Note}, Amount: {Amount}",
                invoice.Status,
                invoice.TransactionStatus,
                invoice.TransactionDate,
                invoice.Note,
                invoice.AmountDue);

            // Verify the invoice exists and is tracked
            var existingInvoice = await _context.Invoices.FindAsync(invoice.Id);
            if (existingInvoice == null)
            {
                _logger.LogError("Failed to update - Invoice with ID: {Id} not found in database", invoice.Id);
                throw new InvalidOperationException($"Invoice with ID {invoice.Id} not found");
            }

            // Update all mutable fields
            existingInvoice.Status = invoice.Status;
            existingInvoice.Note = invoice.Note;
            existingInvoice.TransactionStatus = invoice.TransactionStatus;
            existingInvoice.TransactionDate = invoice.TransactionDate;
            existingInvoice.IssueDate = invoice.IssueDate;
            existingInvoice.DueDate = invoice.DueDate;
            existingInvoice.AmountDue = invoice.AmountDue;
            existingInvoice.TotalAmount = invoice.TotalAmount;
            existingInvoice.TotalVat = invoice.TotalVat;
            existingInvoice.Withholding = invoice.Withholding;
            existingInvoice.ServiceFees = invoice.ServiceFees;
            existingInvoice.TransactionId = invoice.TransactionId;

            // Log the changes being made
            _logger.LogInformation("Updating invoice ID: {Id} with Status: {Status}, Amount: {Amount}",
                invoice.Id, invoice.Status, invoice.AmountDue);

            // Track the number of changes
            var changes = await _context.SaveChangesAsync();

            if (changes > 0)
            {
                _logger.LogInformation("Successfully updated invoice ID: {Id}. Changes saved: {Changes}",
                    invoice.Id, changes);
            }
            else
            {
                _logger.LogWarning("No changes were saved for invoice ID: {Id}", invoice.Id);
            }
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency conflict while updating invoice ID: {Id}", invoice.Id);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while updating invoice ID: {Id}", invoice.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while updating invoice ID: {Id}", invoice.Id);
            throw;
        }
    }

    public async Task AddAsync(Invoice invoice)
    {
        try
        {
            _logger.LogInformation("Adding new invoice with Number: {InvoiceNumber}", invoice.InvoiceNumber);
            await _context.Invoices.AddAsync(invoice);
            var changes = await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully added invoice. Invoice ID: {Id}", invoice.Id);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while adding invoice with Number: {InvoiceNumber}",
                invoice.InvoiceNumber);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding invoice with Number: {InvoiceNumber}",
                invoice.InvoiceNumber);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            _logger.LogInformation("Attempting to delete invoice with ID: {Id}", id);
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
                var changes = await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully deleted invoice with ID: {Id}", id);
            }
            else
            {
                _logger.LogWarning("Delete failed - Invoice with ID: {Id} not found", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting invoice with ID: {Id}", id);
            throw;
        }
    }

    public async Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber)
    {
        try
        {
            _logger.LogInformation("Fetching invoice with Number: {InvoiceNumber}", invoiceNumber);
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);

            if (invoice == null)
            {
                _logger.LogWarning("Invoice with Number: {InvoiceNumber} not found", invoiceNumber);
            }
            else
            {
                _logger.LogInformation("Successfully retrieved invoice with Number: {InvoiceNumber}", invoiceNumber);
            }

            return invoice;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching invoice with Number: {InvoiceNumber}", invoiceNumber);
            throw;
        }
    }

    public async Task<Invoice?> GetByLoadIdAsync(int loadId)
    {
        try
        {
            _logger.LogInformation("Fetching invoice for Load ID: {LoadId}", loadId);
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.LoadId == loadId);

            if (invoice == null)
            {
                _logger.LogWarning("Invoice for Load ID: {LoadId} not found", loadId);
            }
            else
            {
                _logger.LogInformation("Successfully retrieved invoice for Load ID: {LoadId}", loadId);
            }

            return invoice;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching invoice for Load ID: {LoadId}", loadId);
            throw;
        }
    }
}
