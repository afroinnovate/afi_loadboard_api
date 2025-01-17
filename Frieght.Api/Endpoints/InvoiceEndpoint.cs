using AutoMapper;
using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using Frieght.Api.Repositories;

namespace Frieght.Api.Endpoints;


public static class InvoiceEndpoints
{
    public static void MapInvoiceEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/invoices");

        group.MapGet("/", async (IInvoiceRepository repo) =>
        {
            var invoices = await repo.GetAllAsync();
            return Results.Ok(invoices);
        });

        group.MapGet("/{id:int}", async (int id, IInvoiceRepository repo) =>
        {
            var invoice = await repo.GetByIdAsync(id);
            return invoice != null ? Results.Ok(invoice) : Results.NotFound();
        });

        group.MapPost("/", async (InvoiceDto invoiceDto, IInvoiceRepository repo, IMapper mapper) =>
        {
            var invoice = mapper.Map<Invoice>(invoiceDto);
            await repo.AddAsync(invoice);
            return Results.Created($"/api/invoices/{invoice.Id}", invoice);
        });

        group.MapPut("/{id:int}", async (int id, InvoiceDto invoiceDto, IInvoiceRepository repo, IMapper mapper) =>
        {
            var existingInvoice = await repo.GetByIdAsync(id);
            if (existingInvoice == null) return Results.NotFound();

            mapper.Map(invoiceDto, existingInvoice);
            await repo.UpdateAsync(existingInvoice);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, IInvoiceRepository repo) =>
        {
            var invoice = await repo.GetByIdAsync(id);
            if (invoice == null) return Results.NotFound();

            await repo.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}
