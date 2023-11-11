using Frieght.Api.Entities;

namespace Frieght.Api.Repositories;

public interface ICarrierRepository
{
    Task<IEnumerable<Carrier>> GetCarriers();
    Task<Carrier?> GetCarrier(int id);
    Task CreateCarrier(Carrier carrier);
    Task DeleteCarrier(Carrier carrier);
    Task UpdateCarrier(Carrier carrier);
}
