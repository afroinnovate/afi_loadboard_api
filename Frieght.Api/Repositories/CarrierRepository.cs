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

    public async Task CreateCarrier(User carrier)
    {
        context.Add(carrier);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCarrier(User carrier)
    {   
        if (carrier is not null) { 
            context.Remove(carrier);
            await context.SaveChangesAsync();
        }
    }

    public async Task<User?> GetCarrier(string id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetCarriers()
    {
        return await context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<User>> GetCarrierByUserType(string UserType)
    {
        try
        {
            return await context.Users.AsNoTracking().Where(u => u.UserType == UserType).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred while fetching carriers", ex);
        }
    }


    public async Task UpdateCarrier(User carrier)
    {
        context.Update(carrier);
        await context.SaveChangesAsync();
    }
}
