using Frieght.Api.Entities;
using Frieght.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Frieght.Api.Repositories;

public class PaymentMethodRepository : IPaymentMethodRepository
{
    private readonly FrieghtDbContext _context;
    private readonly ILogger<PaymentMethodRepository> _logger;

    public PaymentMethodRepository(FrieghtDbContext context, ILogger<PaymentMethodRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
        try
        {
            return await _context.PaymentMethods.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all payment methods");
            throw;
        }
    }

    public async Task<PaymentMethod?> GetByIdAsync(int id)
    {
        try
        {
            return await _context.PaymentMethods.FirstOrDefaultAsync(p => p.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting payment method with ID: {Id}", id);
            throw;
        }
    }

    public async Task AddAsync(PaymentMethod paymentMethod)
    {
        try
        {
            await _context.PaymentMethods.AddAsync(paymentMethod);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Payment method added successfully: {PaymentMethodId}", paymentMethod.PaymentMethodId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding payment method: {PaymentMethodId}", paymentMethod.PaymentMethodId);
            throw;
        }
    }

    public async Task UpdateAsync(PaymentMethod paymentMethod)
    {
        try
        {
            var existingPaymentMethod = await _context.PaymentMethods
                .FirstOrDefaultAsync(p => p.Id == paymentMethod.Id && p.PaymentMethodId == paymentMethod.PaymentMethodId);

            if (existingPaymentMethod == null)
            {
                _logger.LogWarning("Payment method not found for update. ID: {Id}, PaymentMethodId: {PaymentMethodId}",
                    paymentMethod.Id, paymentMethod.PaymentMethodId);
                throw new KeyNotFoundException($"Payment method not found with ID: {paymentMethod.Id} and PaymentMethodId: {paymentMethod.PaymentMethodId}");
            }

            // Only update fields that have changed
            if (paymentMethod.BankAccount != existingPaymentMethod.BankAccount)
                existingPaymentMethod.BankAccount = paymentMethod.BankAccount;

            if (paymentMethod.LastFourDigits != existingPaymentMethod.LastFourDigits)
                existingPaymentMethod.LastFourDigits = paymentMethod.LastFourDigits;

            if (paymentMethod.PhoneNumber != existingPaymentMethod.PhoneNumber)
                existingPaymentMethod.PhoneNumber = paymentMethod.PhoneNumber;

            // Add any other fields that should be checked for updates

            // Mark entity as modified and save
            _context.Entry(existingPaymentMethod).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Payment method updated successfully. ID: {Id}, PaymentMethodId: {PaymentMethodId}",
                existingPaymentMethod.Id, existingPaymentMethod.PaymentMethodId);
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating payment method. ID: {Id}, PaymentMethodId: {PaymentMethodId}",
                paymentMethod.Id, paymentMethod.PaymentMethodId);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);
            if (paymentMethod != null)
            {
                _context.PaymentMethods.Remove(paymentMethod);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Payment method deleted successfully: {Id}", id);
            }
            else
            {
                _logger.LogWarning("Attempted to delete non-existent payment method with ID: {Id}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting payment method with ID: {Id}", id);
            throw;
        }
    }

    public async Task<PaymentMethod?> GetByPaymentMethodIdAsync(string paymentMethodId)
    {
        try
        {
            return await _context.PaymentMethods
                .FirstOrDefaultAsync(p => p.PaymentMethodId == paymentMethodId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting payment method by PaymentMethodId: {PaymentMethodId}", paymentMethodId);
            throw;
        }
    }

    public async Task<PaymentMethod?> GetByBankAccountAsync(string bankAccount)
    {
        try
        {
            return await _context.PaymentMethods
                .FirstOrDefaultAsync(p => p.BankAccount == bankAccount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting payment method by bank account: {BankAccount}", bankAccount);
            throw;
        }
    }

    public async Task<PaymentMethod?> GetByLastFourDigitsAsync(string lastFourDigits)
    {
        try
        {
            return await _context.PaymentMethods
                .FirstOrDefaultAsync(p => p.LastFourDigits == lastFourDigits);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting payment method by last four digits: {LastFourDigits}", lastFourDigits);
            throw;
        }
    }

    public async Task<PaymentMethod?> GetByPhoneNumberAsync(string phoneNumber)
    {
        try
        {
            return await _context.PaymentMethods
                .FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting payment method by phone number: {PhoneNumber}", phoneNumber);
            throw;
        }
    }
}