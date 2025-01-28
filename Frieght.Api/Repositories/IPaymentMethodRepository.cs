using Frieght.Api.Entities;

namespace Frieght.Api.Repositories;

public interface IPaymentMethodRepository
{
    Task<IEnumerable<PaymentMethod>> GetAllAsync();
    Task<PaymentMethod?> GetByIdAsync(int id);
    Task AddAsync(PaymentMethod paymentMethod);
    Task UpdateAsync(PaymentMethod paymentMethod);
    Task DeleteAsync(int id);
    Task<PaymentMethod?> GetByPaymentMethodIdAsync(string paymentMethodId);
    Task<PaymentMethod?> GetByBankAccountAsync(string bankAccount);
    Task<PaymentMethod?> GetByLastFourDigitsAsync(string lastFourDigits);
    Task<PaymentMethod?> GetByPhoneNumberAsync(string phoneNumber);
}
