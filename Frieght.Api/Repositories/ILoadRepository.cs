using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using System.Collections;

namespace Frieght.Api.Repositories;

public interface ILoadRepository
{
    Task<IEnumerable<Load>> GetLoads();
    Task<Load?> GetLoad(int id);
    Task CreateLoad(CreateLoadDto loadDto);
    Task DeleteLoad(int id);
    Task UpdateLoad(Load load);
}
