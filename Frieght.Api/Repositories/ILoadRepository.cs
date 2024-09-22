using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using System.Collections;

namespace Frieght.Api.Repositories;

public interface ILoadRepository
{
    Task<IEnumerable<Load>> GetLoads();
    Task<Load?> GetLoad(int id);
    Task<IEnumerable<Load?>> GetLoadsByShipper(string shipperId);
    Task CreateLoad(Load load, User shipper);
    Task DeleteLoad(int id);
    Task UpdateLoad(Load load);
}
