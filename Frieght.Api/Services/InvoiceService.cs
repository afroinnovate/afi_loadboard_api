using AutoMapper;
using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Repositories;

namespace Frieght.Api.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _repository;
    private readonly IMapper _mapper;

    public InvoiceService(IInvoiceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InvoiceDto>> GetAllAsync()
    {
        var invoices = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
    }

    public async Task<InvoiceDto?> GetByIdAsync(int id)
    {
        var invoice = await _repository.GetByIdAsync(id);
        return _mapper.Map<InvoiceDto?>(invoice);
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
}

