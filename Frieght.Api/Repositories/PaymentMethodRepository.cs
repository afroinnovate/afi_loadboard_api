using Frieght.Api.Entities;
using Frieght.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Frieght.Api.Repositories;

public class PaymentMethodRepository : IPaymentMethodRepository
{
    private readonly FrieghtDbContext _context;

    public PaymentMethodRepository(FrieghtDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PaymentMethod>> GetAllAsync() => await _context.PaymentMethods.ToListAsync();

    public async Task<PaymentMethod?> GetByIdAsync(int id) => await _context.PaymentMethods.FirstOrDefaultAsync(p => p.Id == id);

    public async Task AddAsync(PaymentMethod paymentMethod)
    {
        await _context.PaymentMethods.AddAsync(paymentMethod);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PaymentMethod paymentMethod)
    {
        _context.PaymentMethods.Update(paymentMethod);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {

        var paymentMethod = await _context.PaymentMethods.FindAsync(id);
        if (paymentMethod != null)
        {
            _context.PaymentMethods.Remove(paymentMethod);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<PaymentMethod?> GetByPaymentMethodIdAsync(string paymentMethodId)
    {
        return await _context.PaymentMethods
            .FirstOrDefaultAsync(p => p.PaymentMethodId == paymentMethodId);
    }

    public async Task<PaymentMethod?> GetByBankAccountAsync(string bankAccount)
    {
        return await _context.PaymentMethods
            .FirstOrDefaultAsync(p => p.BankAccount == bankAccount);
    }

    public async Task<PaymentMethod?> GetByLastFourDigitsAsync(string lastFourDigits)
    {
        return await _context.PaymentMethods
            .FirstOrDefaultAsync(p => p.LastFourDigits == lastFourDigits);
    }

    public async Task<PaymentMethod?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.PaymentMethods
            .FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);
    }
}