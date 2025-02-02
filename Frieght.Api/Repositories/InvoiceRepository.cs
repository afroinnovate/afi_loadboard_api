using Frieght.Api.Entities;
using Frieght.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Frieght.Api.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly FrieghtDbContext _context;
    private readonly IPaymentMethodRepository _paymentRepo;

    public InvoiceRepository(FrieghtDbContext context, IPaymentMethodRepository paymentRepo)
    {
        _context = context;
        _paymentRepo = paymentRepo;
    }

    public async Task<IEnumerable<Invoice>> GetAllAsync()
    {
        var invoices = await _context.Invoices.ToListAsync();
        return invoices;
    }

    public async Task<Invoice?> GetByIdAsync(int id)
    {
        var invoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.Id == id);
        return invoice;
    }

    public async Task<IEnumerable<Invoice>> GetByCarrierIdAsync(string carrierId)
    {
        var invoices = await _context.Invoices
            .Where(i => i.CarrierId == carrierId)
            .ToListAsync();
        return invoices;
    }

    public async Task AddAsync(Invoice invoice)
    {
        await _context.Invoices.AddAsync(invoice);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Invoice invoice)
    {
        _context.Invoices.Update(invoice);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice != null)
        {
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber)
    {
        return await _context.Invoices
            .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
    }

    public async Task<Invoice?> GetByLoadIdAsync(int loadId)
    {
        return await _context.Invoices
            .FirstOrDefaultAsync(i => i.LoadId == loadId);
    }
}
