using AutoMapper;
using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Repositories;

namespace Frieght.Api.Services;

public class PaymentMethodService : IPaymentMethodService
{
    private readonly IPaymentMethodRepository _repository;
    private readonly IMapper _mapper;

    public PaymentMethodService(IPaymentMethodRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PaymentMethodDto>> GetAllAsync()
    {
        var invoices = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<PaymentMethodDto>>(invoices);
    }

    public async Task<PaymentMethodDto?> GetByIdAsync(int id)
    {
        var invoice = await _repository.GetByIdAsync(id);
        return _mapper.Map<PaymentMethodDto?>(invoice);
    }

    public async Task AddAsync(PaymentMethodDto paymentMethodDto)
    {
        var paymentMethod = _mapper.Map<PaymentMethod>(paymentMethodDto);
        await _repository.AddAsync(paymentMethod);
    }

    public async Task UpdateAsync(int id, PaymentMethodDto paymentMethodDto)
    {
        var invoice = await _repository.GetByIdAsync(id);
        if (invoice != null)
        {
            _mapper.Map(paymentMethodDto, invoice);
            await _repository.UpdateAsync(invoice);
        }
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }


}

