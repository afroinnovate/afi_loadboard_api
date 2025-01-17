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
        var payments = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<PaymentMethodDto>>(payments);
    }

    public async Task<PaymentMethodDto?> GetByIdAsync(int id)
    {
        var payments = await _repository.GetByIdAsync(id);
        return _mapper.Map<PaymentMethodDto?>(payments);
    }

    public async Task AddAsync(PaymentMethodDto paymentMethodDto)
    {
        var paymentMethod = _mapper.Map<PaymentMethod>(paymentMethodDto);
        await _repository.AddAsync(paymentMethod);
    }

    public async Task UpdateAsync(int id, PaymentMethodDto paymentMethodDto)
    {
        var payment = await _repository.GetByIdAsync(id);
        if (payment != null)
        {
            _mapper.Map(paymentMethodDto, payment);
            await _repository.UpdateAsync(payment);
        }
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }


}

