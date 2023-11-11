using Frieght.Api.Entities;
using Frieght.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Frieght.Api.Repositories;

public class CarrierRepository : ICarrierRepository
{
    private readonly FrieghtDbContext context;

    public CarrierRepository(FrieghtDbContext context)
    {
        this.context = context;
    }

    public async Task CreateCarrier(Carrier carrier)
    {
        context.Add(carrier);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCarrier(Carrier carrier)
    {   
        if (carrier is not null) { 
            context.Remove(carrier);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Carrier?> GetCarrier(int id)
    {
        return await context.Carriers.FindAsync(id);
    }

    public async Task<IEnumerable<Carrier>> GetCarriers()
    {
        return await context.Carriers.AsNoTracking().ToListAsync();
    }

    public async Task UpdateCarrier(Carrier carrier)
    {
        context.Update(carrier);
        await context.SaveChangesAsync();
    }
}
