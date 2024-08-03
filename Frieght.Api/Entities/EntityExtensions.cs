using Frieght.Api.Dtos;
using Frieght.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Frieght.Api.Extensions
{
    public static class EntityExtensions
    {
        public static CarrierDto AsCarrierDto(this User user)
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
                VehicleTypes: user.BusinessProfile?.BusinessVehicleTypes?.Select(vt => vt.AsVehicleDto()).ToArray() ?? Array.Empty<VehicleTypeDto>(),
                CarrierRole: user.BusinessProfile?.CarrierRole ?? 0
            );
        }

        public static ShipperDto AsShipperDto(this User user)
        {
            return new ShipperDto(
                UserId: user.UserId,
                Email: user.Email,
                FirstName: user.FirstName,
                MiddleName: user.MiddleName,
                LastName: user.LastName,
                Phone: user.Phone,
                UserType: user.UserType,
                BusinessType: user.BusinessProfile?.BusinessType ?? "",
                BusinessRegistrationNumber: user.BusinessProfile?.BusinessRegistrationNumber ?? "",
                CompanyName: user.BusinessProfile?.CompanyName ?? "",
                ShipperRole: user.BusinessProfile?.ShipperRole ?? 0,
                BusinessProfile: user.BusinessProfile?.AsBusinessProfileDto()
            );
        }

        public static User AsUser(this CreateCarrierDto createCarrierDto)
        {
            var businessProfile = new BusinessProfile
            {
                UserId = createCarrierDto.UserId,
                CompanyName = createCarrierDto.CompanyName,
                MotorCarrierNumber = createCarrierDto.MotorCarrierNumber,
                DOTNumber = createCarrierDto.DOTNumber,
                EquipmentType = createCarrierDto.EquipmentType,
                AvailableCapacity = createCarrierDto.AvailableCapacity,
                CarrierRole = createCarrierDto.CarrierRole
            };

            var user = new User
            {
                UserId = createCarrierDto.UserId,
                Email = createCarrierDto.Email,
                FirstName = createCarrierDto.FirstName,
                MiddleName = createCarrierDto.MiddleName,
                LastName = createCarrierDto.LastName,
                Phone = createCarrierDto.Phone,
                UserType = createCarrierDto.UserType,
                BusinessProfile = businessProfile
            };

            businessProfile.BusinessVehicleTypes = new List<BusinessVehicleType>
            {
                new BusinessVehicleType
                {
                    VehicleType = new VehicleType
                    {
                        Name = createCarrierDto.Name,
                        Description = createCarrierDto.Description,
                        ImageUrl = createCarrierDto.ImageUrl,
                        VIN = createCarrierDto.VIN,
                        LicensePlate = createCarrierDto.LicensePlate,
                        Make = createCarrierDto.Make,
                        Model = createCarrierDto.Model,
                        Year = createCarrierDto.Year,
                        Color = createCarrierDto.Color,
                        HasInsurance = createCarrierDto.HasInsurance,
                        HasRegistration = createCarrierDto.HasRegistration,
                        HasInspection = createCarrierDto.HasInspection
                    },
                    Quantity = createCarrierDto.Quantity,
                    BusinessProfile = businessProfile
                }
            };
            return user;
        }

        public static User AsUser(this UpdateCarrierDto updateCarrierDto)
        {
            var businessProfile = new BusinessProfile
            {
                UserId = updateCarrierDto.UserId,
                CompanyName = updateCarrierDto.CompanyName,
                MotorCarrierNumber = updateCarrierDto.MotorCarrierNumber,
                DOTNumber = updateCarrierDto.DOTNumber,
                EquipmentType = updateCarrierDto.EquipmentType,
                AvailableCapacity = updateCarrierDto.AvailableCapacity,
                CarrierRole = updateCarrierDto.CarrierRole
            };

            var user = new User
            {
                UserId = updateCarrierDto.UserId,
                Email = updateCarrierDto.Email,
                FirstName = updateCarrierDto.FirstName,
                MiddleName = updateCarrierDto.MiddleName,
                LastName = updateCarrierDto.LastName,
                Phone = updateCarrierDto.Phone,
                UserType = updateCarrierDto.UserType,
                BusinessProfile = businessProfile
            };

            businessProfile.BusinessVehicleTypes = new List<BusinessVehicleType>
            {
                new BusinessVehicleType
                {
                    VehicleType = new VehicleType
                    {
                        Name = updateCarrierDto.Name,
                        Description = updateCarrierDto.Description,
                        ImageUrl = updateCarrierDto.ImageUrl,
                        VIN = updateCarrierDto.VIN,
                        LicensePlate = updateCarrierDto.LicensePlate,
                        Make = updateCarrierDto.Make,
                        Model = updateCarrierDto.Model,
                        Year = updateCarrierDto.Year,
                        Color = updateCarrierDto.Color,
                        HasInsurance = updateCarrierDto.HasInsurance,
                        HasRegistration = updateCarrierDto.HasRegistration,
                        HasInspection = updateCarrierDto.HasInspection
                    },
                    Quantity = updateCarrierDto.Quantity,
                    BusinessProfile = businessProfile
                }
            };
            return user;
        }

        public static BusinessProfileDto AsBusinessProfileDto(this BusinessProfile businessProfile)
        {
            return new BusinessProfileDto(
                businessProfile.UserId,
                businessProfile.CompanyName,
                businessProfile.MotorCarrierNumber,
                businessProfile.DOTNumber,
                businessProfile.EquipmentType,
                businessProfile.AvailableCapacity,
                businessProfile.IDCardOrDriverLicenceNumber,
                businessProfile.InsuranceName,
                businessProfile.BusinessType,
                businessProfile.CarrierRole,
                businessProfile.ShipperRole,
                businessProfile.BusinessRegistrationNumber,
                businessProfile.BusinessVehicleTypes?.Select(bvt => bvt.AsVehicleDto()).ToList() ?? new List<VehicleTypeDto>()
            );
        }

        public static VehicleTypeDto AsVehicleDto(this BusinessVehicleType businessVehicleType)
        {
            if (businessVehicleType?.VehicleType == null)
            {
                return null;
            }

            return new VehicleTypeDto(
                businessVehicleType.VehicleType.Name,
                businessVehicleType.VehicleType.Description,
                businessVehicleType.VehicleType.ImageUrl,
                businessVehicleType.VehicleType.VIN,
                businessVehicleType.VehicleType.LicensePlate,
                businessVehicleType.VehicleType.Make,
                businessVehicleType.VehicleType.Model,
                businessVehicleType.VehicleType.Year,
                businessVehicleType.VehicleType.Color,
                businessVehicleType.VehicleType.HasInsurance,
                businessVehicleType.VehicleType.HasRegistration,
                businessVehicleType.VehicleType.HasInspection,
                businessVehicleType.Quantity
            );
        }

        public static User AsUser(this CreateUserDto createUserDto)
        {
            var businessProfile = new BusinessProfile
            {
                UserId = createUserDto.UserId,
                CompanyName = createUserDto.CompanyName,
                MotorCarrierNumber = createUserDto.MotorCarrierNumber,
                DOTNumber = createUserDto.DOTNumber,
                EquipmentType = createUserDto.EquipmentType,
                AvailableCapacity = createUserDto.AvailableCapacity,
                CarrierRole = createUserDto.CarrierRole,
                ShipperRole = createUserDto.ShipperRole
            };

            var businessVehicleTypes = createUserDto.VehicleTypes?.Select(vt => new BusinessVehicleType
            {
                VehicleType = new VehicleType
                {
                    Name = vt.Name,
                    Description = vt.Description,
                    ImageUrl = vt.ImageUrl,
                    VIN = vt.VIN,
                    LicensePlate = vt.LicensePlate,
                    Make = vt.Make,
                    Model = vt.Model,
                    Year = vt.Year,
                    Color = vt.Color,
                    HasInsurance = vt.HasInsurance,
                    HasRegistration = vt.HasRegistration,
                    HasInspection = vt.HasInspection
                },
                Quantity = vt.Quantity,
                BusinessProfile = businessProfile
            }).ToList();

            if (businessVehicleTypes != null)
            {
                businessProfile.BusinessVehicleTypes = businessVehicleTypes;
            }

            var user = new User
            {
                UserId = createUserDto.UserId,
                Email = createUserDto.Email,
                FirstName = createUserDto.FirstName,
                MiddleName = createUserDto.MiddleName,
                LastName = createUserDto.LastName,
                Phone = createUserDto.Phone,
                UserType = createUserDto.UserType,
                BusinessProfile = businessProfile
            };

            return user;
        }

        public static User AsUser(this ShipperDto shipperDto)
        {
            var businessProfile = new BusinessProfile
            {
                UserId = shipperDto.UserId,
                CompanyName = shipperDto.CompanyName,
                BusinessType = shipperDto.BusinessType,
                BusinessRegistrationNumber = shipperDto.BusinessRegistrationNumber,
                ShipperRole = shipperDto.ShipperRole
            };

            var user = new User
            {
                UserId = shipperDto.UserId,
                Email = shipperDto.Email,
                FirstName = shipperDto.FirstName,
                MiddleName = shipperDto.MiddleName,
                LastName = shipperDto.LastName,
                Phone = shipperDto.Phone,
                UserType = "Shipper",
                BusinessProfile = businessProfile
            };
            return user;
        }

        public static User AsCarrierUser(this CarrierDto carrierDto)
        {
            var businessProfile = new BusinessProfile
            {
                UserId = carrierDto.UserId,
                CompanyName = carrierDto.CompanyName,
                MotorCarrierNumber = carrierDto.MotorCarrierNumber,
                DOTNumber = carrierDto.DOTNumber,
                EquipmentType = carrierDto.EquipmentType,
                AvailableCapacity = carrierDto.AvailableCapacity,
                CarrierRole = carrierDto.CarrierRole
            };

            var businessVehicleTypes = carrierDto.VehicleTypes?.Select(vt => new BusinessVehicleType
            {
                VehicleType = new VehicleType
                {
                    Name = vt.Name,
                    Description = vt.Description,
                    ImageUrl = vt.ImageUrl,
                    VIN = vt.VIN,
                    LicensePlate = vt.LicensePlate,
                    Make = vt.Make,
                    Model = vt.Model,
                    Year = vt.Year,
                    Color = vt.Color,
                    HasInsurance = vt.HasInsurance,
                    HasRegistration = vt.HasRegistration,
                    HasInspection = vt.HasInspection
                },
                Quantity = vt.Quantity,
                BusinessProfile = businessProfile
            }).ToList();

            if (businessVehicleTypes != null)
            {
                businessProfile.BusinessVehicleTypes = businessVehicleTypes;
            }

            var user = new User
            {
                UserId = carrierDto.UserId,
                Email = carrierDto.Email,
                FirstName = carrierDto.FirstName,
                MiddleName = carrierDto.MiddleName,
                LastName = carrierDto.LastName,
                Phone = carrierDto.Phone,
                UserType = "Carrier",
                BusinessProfile = businessProfile
            };

            return user;
        }

        public static LoadDto AsLoadDto(this Load load)
        {
            return new LoadDto(
                load.LoadId,
                load.ShipperUserId,
                load.Shipper.AsShipperDto(), // Transform User entity to ShipperDto
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

        public static Load AsCreateLoad(this CreateLoadDto createLoadDto)
        {
            return new Load
            {
                ShipperUserId = createLoadDto.ShipperUserId,
                Origin = createLoadDto.Origin,
                Destination = createLoadDto.Destination,
                PickupDate = createLoadDto.PickupDate,
                DeliveryDate = createLoadDto.DeliveryDate,
                Commodity = createLoadDto.Commodity,
                Weight = createLoadDto.Weight,
                OfferAmount = createLoadDto.OfferAmount,
                LoadDetails = createLoadDto.LoadDetails,
                LoadStatus = createLoadDto.LoadStatus,
                CreatedAt = createLoadDto.CreatedAt,
                Shipper = createLoadDto.CreatedBy.AsUser()
            };
        }

        public static Load AsLoad(this LoadDto loadDto)
        {
            return new Load
            {
                LoadId = loadDto.LoadId,
                ShipperUserId = loadDto.ShipperUserId,
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
                ModifiedAt = loadDto.ModifiedAt,
                ModifiedBy = loadDto.ModifiedBy,
                Shipper = loadDto.CreatedBy.AsUser()
            };
        }

        public static BidDto AsBidDto(this Bid bid)
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
                bid.Load.AsLoadDto(),     // Transform Load entity to LoadDto
                bid.Carrier.AsCarrierDto()   // Transform User entity (acting as Carrier) to CarrierDto
            );
        }
    }
}
