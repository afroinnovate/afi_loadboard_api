using Frieght.Api.Entities;

namespace Frieght.Api.Repositories;

public interface ICarrierRepository
{
    Task<IEnumerable<User>> GetCarriers();
    Task<User?> GetCarrier(string id);
    Task CreateCarrier(User carrier);
    Task DeleteCarrier(User carrier);
    Task UpdateCarrier(User carrier);
}
