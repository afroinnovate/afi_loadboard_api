using Frieght.Api.Entities;
using Frieght.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace Frieght.Api.Repositories
{
    public class LoadRepository : ILoadRepository
    {
        private readonly FrieghtDbContext context;

        public LoadRepository(FrieghtDbContext context)
        {
            this.context = context;
        }
        public async Task CreateLoad(Load load)
        {
           context.Add(load);
           await context.SaveChangesAsync();
        }

        public async Task DeleteLoad(int id)
        {
            var load = context.Loads.Find(id);
            if(load is not null) context.Remove(load);
            await context.SaveChangesAsync();
        }

        public async Task<Load?> GetLoad(int id)
        {
           return await context.Loads.FindAsync(id);
        }

        public async Task<IEnumerable<Load>> GetLoads()
        {
            return await context.Loads.AsNoTracking().ToListAsync();
        }

        public async Task UpdateLoad(Load load)
        {
            context.Update(load);
            await context.SaveChangesAsync();
        }
    }
}
