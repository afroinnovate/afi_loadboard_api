using Frieght.Api.Dtos;

namespace Frieght.Api.Services;

public interface IPaymentMethodService
{
    Task<IEnumerable<PaymentMethodDto>> GetAllAsync();
    Task<PaymentMethodDto?> GetByIdAsync(int id);
    Task AddAsync(PaymentMethodDto paymentMethodDto);
    Task UpdateAsync(int id, PaymentMethodDto paymentMethodDto);
    Task DeleteAsync(int id);
}
