using Frieght.Api.Dtos;

namespace Frieght.Api.Entities;

public static class EntityExtensions
{
    public static LoadDto asDto(this Load load)
    {
        return new LoadDto(
                   load.Id,
                   load.UserId,
                   new ShipperDto(load.ShipperUserId, load.Shipper?.Email, load.Shipper?.FirstName, load.Shipper?.LastName, load.Shipper?.CompanyName, load.Shipper?.DOTNumber),
                   load.Origin,
                   load.destination,
                   load.PickupDate,
                   load.DeliveryDate,
                   load.Commodity,
                   load.Weight,
                   load.OfferAmount,
                   load.LoadDetails,
                   load.LoadStatus,
                   load.AcceptedBidId,
                   load.Created,
                   load.Modified,
                   load.ModifiedBy
            );
    }

    public static CarrierDto asDto(this Carrier carrier)
    {
        return new CarrierDto(
            carrier.Id,
            carrier.UserId,
            carrier.CompanyName,
            carrier.CompanyEmail,
            carrier.CompanyPhone,
            carrier.MotorCarrierNumber,
            carrier.DOTNumber,
            carrier.EquipmentType,
            carrier.AvailableCapacity
            );
    }

    public static BidDto asDto(this Bid bid)
    {
        return new BidDto(
                   bid.Id,
                   bid.LoadId,
                   bid.CarrierId,
                   bid.BidAmount,
                   bid.BidStatus,
                   bid.BiddingTime,
                   bid.UpdatedAt,
                   bid.UpdatedBy
            );
    }

}
