using AutoMapper;
using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Repositories;

namespace Frieght.Api.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _repository;
    private readonly IPaymentMethodRepository _paymentRepo;
    private readonly IMapper _mapper;

    public InvoiceService(
        IInvoiceRepository repository,
        IPaymentMethodRepository paymentRepo,
        IMapper mapper)
    {
        _repository = repository;
        _paymentRepo = paymentRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InvoiceDto>> GetAllAsync()
    {
        var invoices = await _repository.GetAllAsync();
        var dtos = new List<InvoiceDto>();

        foreach (var invoice in invoices)
        {
            var dto = _mapper.Map<InvoiceDto>(invoice);
            if (!string.IsNullOrEmpty(invoice.PaymentMethodId))
            {
                var payment = await _paymentRepo.GetByPaymentMethodIdAsync(invoice.PaymentMethodId);
                dto.PaymentMethod = _mapper.Map<PaymentMethodDto>(payment);
            }
            dtos.Add(dto);
        }

        return dtos;
    }

    public async Task<InvoiceDto?> GetByIdAsync(int id)
    {
        var invoice = await _repository.GetByIdAsync(id);
        if (invoice == null) return null;

        var dto = _mapper.Map<InvoiceDto>(invoice);
        if (!string.IsNullOrEmpty(invoice.PaymentMethodId))
        {
            var payment = await _paymentRepo.GetByPaymentMethodIdAsync(invoice.PaymentMethodId);
            dto.PaymentMethod = _mapper.Map<PaymentMethodDto>(payment);
        }
        return dto;
    }

    public async Task AddAsync(InvoiceDto invoiceDto)
    {
        var invoice = _mapper.Map<Invoice>(invoiceDto);
        await _repository.AddAsync(invoice);
    }

    public async Task UpdateAsync(int id, InvoiceDto invoiceDto)
    {
        var invoice = await _repository.GetByIdAsync(id);
        if (invoice != null)
        {
            _mapper.Map(invoiceDto, invoice);
            await _repository.UpdateAsync(invoice);
        }
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<InvoiceDto>> GetByCarrierIdAsync(string carrierId)
    {
        var invoices = await _repository.GetByCarrierIdAsync(carrierId);
        var dtos = new List<InvoiceDto>();

        foreach (var invoice in invoices)
        {
            var dto = _mapper.Map<InvoiceDto>(invoice);
            if (!string.IsNullOrEmpty(invoice.PaymentMethodId))
            {
                var payment = await _paymentRepo.GetByPaymentMethodIdAsync(invoice.PaymentMethodId);
                dto.PaymentMethod = _mapper.Map<PaymentMethodDto>(payment);
            }
            dtos.Add(dto);
        }

        return dtos;
    }

    public async Task<InvoiceDto?> GetByInvoiceNumberAsync(string invoiceNumber)
    {
        var invoice = await _repository.GetByInvoiceNumberAsync(invoiceNumber);
        if (invoice == null) return null;

        var dto = _mapper.Map<InvoiceDto>(invoice);
        if (!string.IsNullOrEmpty(invoice.PaymentMethodId))
        {
            var payment = await _paymentRepo.GetByPaymentMethodIdAsync(invoice.PaymentMethodId);
            dto.PaymentMethod = _mapper.Map<PaymentMethodDto>(payment);
        }
        return dto;
    }

    public async Task<InvoiceDto?> GetByLoadIdAsync(int loadId)
    {
        var invoice = await _repository.GetByLoadIdAsync(loadId);
        if (invoice == null) return null;

        var dto = _mapper.Map<InvoiceDto>(invoice);
        if (!string.IsNullOrEmpty(invoice.PaymentMethodId))
        {
            var payment = await _paymentRepo.GetByPaymentMethodIdAsync(invoice.PaymentMethodId);
            dto.PaymentMethod = _mapper.Map<PaymentMethodDto>(payment);
        }
        return dto;
    }
}

