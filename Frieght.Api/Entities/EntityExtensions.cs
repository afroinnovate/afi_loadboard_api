using Frieght.Api.Dtos;

namespace Frieght.Api.Entities;

public static class EntityExtensions
{
    public static LoadDto asDto(this Load load)
    {
        return new LoadDto(
            load.LoadId,
            load.ShipperUserId,
            load.Shipper?.asShipperDto(), // Transform User entity to ShipperDto
            load.Origin,
            load.Destination,
            load.PickupDate,
            load.DeliveryDate,
            load.Commodity,
            load.Weight,
            load.OfferAmount,
            load.LoadDetails,
            load.LoadStatus,
            load.CreatedAt,
            load.ModifiedAt,
            load.ModifiedBy
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
            bid.UpdatedBy,
            bid.Load?.asDto(),     // Transform Load entity to LoadDto
            bid.Carrier?.asCarrierDto()   // Transform User entity (acting as Carrier) to CarrierDto
        );
    }

    public static CarrierDto asCarrierDto(this User user)
    {
        return new CarrierDto(
            user.UserId, // Unique identifier
            user.Email,
            user.FirstName,
            user.LastName,
            user.Phone,
            user.DOTNumber,
            user.MotorCarrierNumber,
            user.EquipmentType,
            user.AvailableCapacity,
            user.CompanyName,
            user.UserType,
            user.CarrierRole,
            user.ShipperRole
        );
    }

    public static ShipperDto asShipperDto(this User user)
    {
        return new ShipperDto(
            user.UserId,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Phone,
            user.CompanyName, // Assuming Shippers can also have company names
            user.DOTNumber
        );
    }

}
