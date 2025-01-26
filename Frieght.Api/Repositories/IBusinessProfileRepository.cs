using Frieght.Api.Entities;
namespace Frieght.Api.Repositories;

public interface IBusinessProfileRepository
{
  Task<BusinessProfile?> GetBusinessProfileByUserId(string userId);
  Task UpdateBusinessProfile(BusinessProfile businessProfile);
}

