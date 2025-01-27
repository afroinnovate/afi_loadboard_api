using Frieght.Api.Entities;

namespace Frieght.Api.Repositories;

public interface IInvoiceRepository
{
    Task<IEnumerable<Invoice>> GetAllAsync();
    Task<Invoice?> GetByIdAsync(int id);
    Task<IEnumerable<Invoice>> GetByCarrierIdAsync(string carrierId);
    Task AddAsync(Invoice invoice);
    Task UpdateAsync(Invoice invoice);
    Task DeleteAsync(int id);
    Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber);
    Task<Invoice?> GetByLoadIdAsync(int loadId);
}
