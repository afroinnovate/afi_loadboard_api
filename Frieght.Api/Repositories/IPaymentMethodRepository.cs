using Frieght.Api.Entities;

namespace Frieght.Api.Repositories;

public interface IPaymentMethodRepository
{
    Task<IEnumerable<PaymentMethod>> GetAllAsync();
    Task<PaymentMethod?> GetByIdAsync(int id);
    Task AddAsync(PaymentMethod paymentMethod);
    Task UpdateAsync(PaymentMethod paymentMethod);
    Task DeleteAsync(int id);
}
