using Frieght.Api.Dtos;
using Frieght.Api.Enuns;

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

    public static Load asLoad(this LoadDto loadDto)
    {
        return new Load
        {
            LoadId = loadDto.LoadId,
            ShipperUserId = loadDto.ShipperUserId,
            Shipper = new User
            {
                UserId = loadDto.ShipperUserId,
                Email = loadDto.CreatedBy.Email,
                FirstName = loadDto.CreatedBy.FirstName,
                MiddleName = loadDto.CreatedBy.MiddleName,
                LastName = loadDto.CreatedBy.LastName,
                UserType = "shipper"
            },
            Origin = loadDto.Origin,
            Destination = loadDto.Destination,
            PickupDate = loadDto.PickupDate,
            DeliveryDate = loadDto.DeliveryDate,
            Commodity = loadDto.Commodity,
            Weight = loadDto.Weight,
            OfferAmount = loadDto.OfferAmount,
            LoadDetails = loadDto.LoadDetails,
            LoadStatus = loadDto.LoadStatus,
            CreatedAt = (DateTime)loadDto.CreatedAt,
            ModifiedAt = (DateTime)loadDto.ModifiedAt,
            ModifiedBy = loadDto.ModifiedBy
        };
    }

    public static BidDto asBidDto(this Bid bid)
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
            bid.Load.asDto(),     // Transform Load entity to LoadDto
            bid.Carrier.asUserDto()   // Transform User entity (acting as Carrier) to CarrierDto
        );
    }

    public static Bid asBid(this CreateBidDto createBidDto)
    {
        return new Bid
        {
            LoadId = createBidDto.LoadId,
            CarrierId = createBidDto.CarrierId,
            BidAmount = createBidDto.BidAmount,
            BidStatus = createBidDto.BidStatus,
            BiddingTime = createBidDto.BiddingTime,
            UpdatedBy = createBidDto.CreatedBy.Email,
            Load = new Load
            {
                LoadId = createBidDto.LoadDto.LoadId,
                ShipperUserId = createBidDto.LoadDto.ShipperUserId,
                Origin = createBidDto.LoadDto.Origin,
                Destination = createBidDto.LoadDto.Destination,
                PickupDate = createBidDto.LoadDto.PickupDate,
                DeliveryDate = createBidDto.LoadDto.DeliveryDate,
                Commodity = createBidDto.LoadDto.Commodity,
                Weight = createBidDto.LoadDto.Weight,
                OfferAmount = createBidDto.LoadDto.OfferAmount,
                LoadDetails = createBidDto.LoadDto.LoadDetails,
                LoadStatus = createBidDto.LoadDto.LoadStatus,
                CreatedAt = (DateTime)createBidDto.LoadDto.CreatedAt,
                Shipper = new User
                {
                    UserId = createBidDto.CreatedBy.UserId,
                    Email = createBidDto.CreatedBy.Email,
                    FirstName = createBidDto.CreatedBy.FirstName,
                    MiddleName = createBidDto.CreatedBy.MiddleName,
                    LastName = createBidDto.CreatedBy.LastName,
                    UserType = "shipper"
                }
            },
            Carrier = new User
            {
                UserId = createBidDto.CarrierId,
                Email = createBidDto.CreatedBy.Email,
                FirstName = createBidDto.CreatedBy.FirstName,
                MiddleName = createBidDto.CreatedBy.MiddleName,
                LastName = createBidDto.CreatedBy.LastName,
                UserType = "carrier"
            }
        };
    }

    public static ShipperDto asShipperDto(this User user)
    {
        return new ShipperDto(
            UserId: user.UserId,
            Email: user.Email,
            FirstName: user.FirstName,
            MiddleName: user.MiddleName,
            LastName: user.LastName,
            Phone: user.Phone,
            UserType: user.UserType,
            BusinessType: user.BusinessProfile?.BusinessType,
            BusinessRegistrationNumber: user.BusinessProfile?.BusinessRegistrationNumber,
            CompanyName: user.BusinessProfile?.CompanyName,
            ShipperRole: user.BusinessProfile?.ShipperRole ?? user.BusinessProfile?.ShipperRole ?? 0,
            BusinessProfile: user.BusinessProfile?.asDto()
        );
    }

    public static CarrierDto asCarrierDto(this User user)
    {
        return new CarrierDto(
            UserId: user.UserId,
            Email: user.Email,
            FirstName: user.FirstName,
            MiddleName: user.MiddleName,
            LastName: user.LastName,
            Phone: user.Phone,
            UserType: user.UserType,
            DOTNumber: user.BusinessProfile?.DOTNumber,
            MotorCarrierNumber: user.BusinessProfile?.MotorCarrierNumber,
            EquipmentType: user.BusinessProfile?.EquipmentType,
            AvailableCapacity: user.BusinessProfile?.AvailableCapacity,
            CompanyName: user.BusinessProfile?.CompanyName,
            VehicleTypes: user.BusinessProfile?.BusinessVehicleTypes?.Select(vt => vt.asDto()).ToArray(),
            CarrierRole: user.BusinessProfile?.CarrierRole ?? user.BusinessProfile?.CarrierRole ?? 0,
            BusinessProfile: user.BusinessProfile?.asDto()
        );
    }

    public static UserDto asUserDto(this User user)
    {
        return new UserDto(
            user.UserId,
            user.Email,
            user.FirstName,
            user.LastName,
            user.MiddleName,
            user.Phone,
            user.UserType,
            user.BusinessProfile?.asDto()
        );
    }

    public static BusinessProfileDto asDto(this BusinessProfile businessProfile)
    {
        return new BusinessProfileDto
        {
            UserId = businessProfile.UserId,
            CompanyName = businessProfile.CompanyName,
            MotorCarrierNumber = businessProfile.MotorCarrierNumber,
            DOTNumber = businessProfile.DOTNumber,
            EquipmentType = businessProfile.EquipmentType,
            AvailableCapacity = businessProfile.AvailableCapacity,
            IDCardOrDriverLicenceNumber = businessProfile.IDCardOrDriverLicenceNumber,
            InsuranceName = businessProfile.InsuranceName,
            BusinessType = businessProfile.BusinessType,
            CarrierRole = businessProfile.CarrierRole,
            ShipperRole = businessProfile.ShipperRole,
            BusinessRegistrationNumber = businessProfile.BusinessRegistrationNumber,
            VehicleTypes = businessProfile.BusinessVehicleTypes?.Select(bvt => bvt.asDto()).ToList()
        };
    }

    public static VehicleTypeDto asDto(this BusinessVehicleType businessVehicleType)
    {
        return new VehicleTypeDto
        {
            Name = businessVehicleType.VehicleType.Name,
            Quantity = businessVehicleType.Quantity,
            HasInsurance = businessVehicleType.VehicleType.HasInsurance,
            HasRegistration = businessVehicleType.VehicleType.HasRegistration,
            HasInspection = businessVehicleType.VehicleType.HasInspection,
            Description = businessVehicleType.VehicleType.Description,
            ImageUrl = businessVehicleType.VehicleType.ImageUrl,
            VIN = businessVehicleType.VehicleType.VIN,
            LicensePlate = businessVehicleType.VehicleType.LicensePlate,
            Make = businessVehicleType.VehicleType.Make,
            Model = businessVehicleType.VehicleType.Model,
            Year = businessVehicleType.VehicleType.Year,
            Color = businessVehicleType.VehicleType.Color
        };
    }
}
