using Frieght.Api.Dtos;

namespace Frieght.Api.Services;

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceDto>> GetAllAsync();
    Task<InvoiceDto?> GetByIdAsync(int id);
    Task AddAsync(InvoiceDto invoiceDto);
    Task UpdateAsync(int id, InvoiceDto invoiceDto);
    Task DeleteAsync(int id);
}
