﻿// using Frieght.Api.Dtos;
// using Frieght.Api.Entities;
// using System;
// using System.Collections.Generic;
// using System.Linq;

// namespace Frieght.Api.Extensions
// {
//     public static class EntityExtensions
//     {
//         public static CarrierDto AsCarrierDto(this User user)
//         {
//             return new CarrierDto(
//                 UserId: user.UserId,
//                 Email: user.Email,
//                 FirstName: user.FirstName,
//                 MiddleName: user.MiddleName,
//                 LastName: user.LastName,
//                 Phone: user.Phone,
//                 UserType: user.UserType,
//                 DOTNumber: user.BusinessProfile?.DOTNumber,
//                 MotorCarrierNumber: user.BusinessProfile?.MotorCarrierNumber,
//                 EquipmentType: user.BusinessProfile?.EquipmentType,
//                 AvailableCapacity: user.BusinessProfile?.AvailableCapacity,
//                 CompanyName: user.BusinessProfile?.CompanyName,
//                 VehicleTypes: user.BusinessProfile?.CarrierVehicles?.Select(vt => vt.AsVehicleDto()).ToArray() ?? Array.Empty<VehicleTypeDto>(),
//                 CarrierRole: user.BusinessProfile?.CarrierRole ?? 0
//             );
//         }

//         public static ShipperDto AsShipperDto(this User user)
//         {
//             return new ShipperDto(
//                 UserId: user.UserId,
//                 Email: user.Email,
//                 FirstName: user.FirstName,
//                 MiddleName: user.MiddleName,
//                 LastName: user.LastName,
//                 Phone: user.Phone,
//                 UserType: user.UserType,
//                 BusinessType: user.BusinessProfile?.BusinessType ?? "",
//                 BusinessRegistrationNumber: user.BusinessProfile?.BusinessRegistrationNumber ?? "",
//                 CompanyName: user.BusinessProfile?.CompanyName ?? "",
//                 ShipperRole: user.BusinessProfile?.ShipperRole ?? 0,
//                 BusinessProfile: user.BusinessProfile?.AsBusinessProfileDto()
//             );
//         }

//         public static User AsUser(this CreateCarrierDto createCarrierDto)
//         {
//             var businessProfile = new BusinessProfile
//             {
//                 UserId = createCarrierDto.UserId,
//                 CompanyName = createCarrierDto.CompanyName,
//                 MotorCarrierNumber = createCarrierDto.MotorCarrierNumber,
//                 DOTNumber = createCarrierDto.DOTNumber,
//                 EquipmentType = createCarrierDto.EquipmentType,
//                 AvailableCapacity = createCarrierDto.AvailableCapacity,
//                 CarrierRole = createCarrierDto.CarrierRole
//             };

//             var user = new User
//             {
//                 UserId = createCarrierDto.UserId,
//                 Email = createCarrierDto.Email,
//                 FirstName = createCarrierDto.FirstName,
//                 MiddleName = createCarrierDto.MiddleName,
//                 LastName = createCarrierDto.LastName,
//                 Phone = createCarrierDto.Phone,
//                 UserType = createCarrierDto.UserType,
//                 BusinessProfile = businessProfile
//             };

//             var VehicleType = new VehicleType
//             {
//                 Name = createCarrierDto.VehicleTypeName
//             };

//             businessProfile.CarrierVehicles = new List<Vehicle>
//             {
//                 new Vehicle
//                 {
//                     Name = createCarrierDto.Name,
//                     Description = createCarrierDto.Description,
//                     ImageUrl = createCarrierDto.ImageUrl,
//                     VIN = createCarrierDto.VIN,
//                     LicensePlate = createCarrierDto.LicensePlate,
//                     Make = createCarrierDto.Make,
//                     Model = createCarrierDto.Model,
//                     Year = createCarrierDto.Year,
//                     Color = createCarrierDto.Color,
//                     HasInsurance = createCarrierDto.HasInsurance,
//                     HasRegistration = createCarrierDto.HasRegistration,
//                     HasInspection = createCarrierDto.HasInspection,
//                     BusinessProfile = businessProfile,
//                     VehicleType = VehicleType
//                 }
//             };
//             return user;
//         }

//         public static User AsUser(this UpdateCarrierDto updateCarrierDto)
//         {
//             var businessProfile = new BusinessProfile
//             {
//                 UserId = updateCarrierDto.UserId,
//                 CompanyName = updateCarrierDto.CompanyName,
//                 MotorCarrierNumber = updateCarrierDto.MotorCarrierNumber,
//                 DOTNumber = updateCarrierDto.DOTNumber,
//                 EquipmentType = updateCarrierDto.EquipmentType,
//                 AvailableCapacity = updateCarrierDto.AvailableCapacity,
//                 CarrierRole = updateCarrierDto.CarrierRole
//             };

//             var user = new User
//             {
//                 UserId = updateCarrierDto.UserId,
//                 Email = updateCarrierDto.Email,
//                 FirstName = updateCarrierDto.FirstName,
//                 MiddleName = updateCarrierDto.MiddleName,
//                 LastName = updateCarrierDto.LastName,
//                 Phone = updateCarrierDto.Phone,
//                 UserType = updateCarrierDto.UserType,
//                 BusinessProfile = businessProfile
//             };

//             var VehicleType = new VehicleType
//             {
//                 Name = updateCarrierDto.VehicleTypeName
//             };

//             businessProfile.CarrierVehicles = new List<Vehicle>
//             {
//                 new Vehicle
//                 {
//                     Name = updateCarrierDto.Name,
//                     Description = updateCarrierDto.Description,
//                     ImageUrl = updateCarrierDto.ImageUrl,
//                     VIN = updateCarrierDto.VIN,
//                     LicensePlate = updateCarrierDto.LicensePlate,
//                     Make = updateCarrierDto.Make,
//                     Model = updateCarrierDto.Model,
//                     Year = updateCarrierDto.Year,
//                     Color = updateCarrierDto.Color,
//                     HasInsurance = updateCarrierDto.HasInsurance,
//                     HasRegistration = updateCarrierDto.HasRegistration,
//                     HasInspection = updateCarrierDto.HasInspection,
//                     BusinessProfile = businessProfile,
//                     VehicleType = VehicleType
//                 }
//             };
//             return user;
//         }
        
//         public static VehicleDto AsVehicleDto(this Vehicle vehicle)
//         {
//             return new VehicleDto(
//                 Name: vehicle.Name?? "",
//                 Description: vehicle.Description ?? "",
//                 ImageUrl: vehicle.ImageUrl ?? "",
//                 VIN: vehicle.VIN ?? "",
//                 LicensePlate: vehicle.LicensePlate ?? "",
//                 Make: vehicle.Make ?? "",
//                 Model: vehicle.Model ?? "",
//                 Year: vehicle.Year ?? "",
//                 Color: vehicle.Color ?? "",
//                 HasInsurance: vehicle.HasInsurance ?? false,
//                 HasRegistration: vehicle.HasRegistration ?? false,
//                 HasInspection: vehicle.HasInspection ?? false,
//                 BusinessProfile: vehicle.BusinessProfile?.AsBusinessProfileDto(),
//                 VehicleType: vehicle.VehicleType.AsVehicleTypeDto()
//             );
//         }

//         public static VehicleTypeDto AsVehicleTypeDto(this VehicleType vehicleType)
//         {
//             return new VehicleTypeDto(
//                 Name: vehicleType.Name,
//                 Vehicles: vehicleType.Vehicles?.Select(v => v.AsVehicleDto()).ToArray() ?? Array.Empty<VehicleDto>()
//             );
//         }

//         public static BusinessProfileDto AsBusinessProfileDto(this BusinessProfile businessProfile)
//         {
//             return new BusinessProfileDto(
//                 businessProfile.UserId,
//                 businessProfile.CompanyName,
//                 businessProfile.MotorCarrierNumber,
//                 businessProfile.DOTNumber,
//                 businessProfile.EquipmentType,
//                 businessProfile.AvailableCapacity,
//                 businessProfile.IDCardOrDriverLicenceNumber,
//                 businessProfile.InsuranceName,
//                 businessProfile.BusinessType,
//                 businessProfile.CarrierRole,
//                 businessProfile.ShipperRole,
//                 businessProfile.BusinessRegistrationNumber,
//                 businessProfile.CarrierVehicles?.Select(v => v.AsVehicleDto()).ToList()
//             );
//         }

//         public static User AsUser(this CreateUserDto createUserDto)
//         {
//             var businessProfile = new BusinessProfile
//             {
//                 UserId = createUserDto.UserId,
//                 CompanyName = createUserDto.CompanyName,
//                 MotorCarrierNumber = createUserDto.MotorCarrierNumber,
//                 DOTNumber = createUserDto.DOTNumber,
//                 EquipmentType = createUserDto.EquipmentType,
//                 AvailableCapacity = createUserDto.AvailableCapacity,
//                 CarrierRole = createUserDto.CarrierRole,
//                 ShipperRole = createUserDto.ShipperRole,
//             };

//             // if carrierVehicles is one or more, add them to the businessProfile
//             if (createUserDto.BusinessProfile.CarrierVehicles != null)
//             {   
//                 IEnumerable<Vehicle> carrierVehicles = createUserDto.BusinessProfile.CarrierVehicles.Select(v => new Vehicle
//                 {
//                     Name = v.Name,
//                     Description = v.Description,
//                     ImageUrl = v.ImageUrl,
//                     VIN = v.VIN,
//                     LicensePlate = v.LicensePlate,
//                     Make = v.Make,
//                     Model = v.Model,
//                     Year = v.Year,
//                     Color = v.Color,
//                     HasInsurance = v.HasInsurance,
//                     HasRegistration = v.HasRegistration,
//                     HasInspection = v.HasInspection
//                 });
//                 while(createUserDto.BusinessProfile.CarrierVehicles.Count > 0)
//                 {
//                     var vehicle = createUserDto.BusinessProfile.CarrierVehicles[0];
//                     businessProfile.CarrierVehicles.Add(new Vehicle
//                     {
//                         Name = vehicle.Name,
//                         Description = vehicle.Description,
//                         ImageUrl = vehicle.ImageUrl,
//                         VIN = vehicle.VIN,
//                         LicensePlate = vehicle.LicensePlate,
//                         Make = vehicle.Make,
//                         Model = vehicle.Model,
//                         Year = vehicle.Year,
//                         Color = vehicle.Color,
//                         HasInsurance = vehicle.HasInsurance,
//                         HasRegistration = vehicle.HasRegistration,
//                         HasInspection = vehicle.HasInspection
//                     });
//                     createUserDto.BusinessProfile.CarrierVehicles.RemoveAt(0);
//                 }
//             }

//             var carrierVehicle = 
//             var user = new User
//             {
//                 UserId = createUserDto.UserId,
//                 Email = createUserDto.Email,
//                 FirstName = createUserDto.FirstName,
//                 MiddleName = createUserDto.MiddleName,
//                 LastName = createUserDto.LastName,
//                 Phone = createUserDto.Phone,
//                 UserType = createUserDto.UserType,
//                 BusinessProfile = businessProfile
//             };

//             return user;
//         }

//         public static User AsUser(this ShipperDto shipperDto)
//         {
//             var businessProfile = new BusinessProfile
//             {
//                 UserId = shipperDto.UserId,
//                 CompanyName = shipperDto.CompanyName,
//                 BusinessType = shipperDto.BusinessType,
//                 BusinessRegistrationNumber = shipperDto.BusinessRegistrationNumber,
//                 ShipperRole = shipperDto.ShipperRole
//             };

//             var user = new User
//             {
//                 UserId = shipperDto.UserId,
//                 Email = shipperDto.Email,
//                 FirstName = shipperDto.FirstName,
//                 MiddleName = shipperDto.MiddleName,
//                 LastName = shipperDto.LastName,
//                 Phone = shipperDto.Phone,
//                 UserType = "Shipper",
//                 BusinessProfile = businessProfile
//             };
//             return user;
//         }

//         public static User AsCarrierUser(this CarrierDto carrierDto)
//         {
//             var businessProfile = new BusinessProfile
//             {
//                 UserId = carrierDto.UserId,
//                 CompanyName = carrierDto.CompanyName,
//                 MotorCarrierNumber = carrierDto.MotorCarrierNumber,
//                 DOTNumber = carrierDto.DOTNumber,
//                 EquipmentType = carrierDto.EquipmentType,
//                 AvailableCapacity = carrierDto.AvailableCapacity,
//                 CarrierRole = carrierDto.CarrierRole
//             };

//             var businessVehicleTypes = carrierDto.VehicleTypes?.Select(vt => new BusinessVehicleType
//             {
//                 VehicleType = new VehicleType
//                 {
//                     Name = vt.Name,
//                     Description = vt.Description,
//                     ImageUrl = vt.ImageUrl,
//                     VIN = vt.VIN,
//                     LicensePlate = vt.LicensePlate,
//                     Make = vt.Make,
//                     Model = vt.Model,
//                     Year = vt.Year,
//                     Color = vt.Color,
//                     HasInsurance = vt.HasInsurance,
//                     HasRegistration = vt.HasRegistration,
//                     HasInspection = vt.HasInspection
//                 },
//                 Quantity = vt.Quantity,
//                 BusinessProfile = businessProfile
//             }).ToList();

//             if (businessVehicleTypes != null)
//             {
//                 businessProfile.BusinessVehicleTypes = businessVehicleTypes;
//             }

//             var user = new User
//             {
//                 UserId = carrierDto.UserId,
//                 Email = carrierDto.Email,
//                 FirstName = carrierDto.FirstName,
//                 MiddleName = carrierDto.MiddleName,
//                 LastName = carrierDto.LastName,
//                 Phone = carrierDto.Phone,
//                 UserType = "Carrier",
//                 BusinessProfile = businessProfile
//             };

//             return user;
//         }

//         public static LoadDtoResponse AsLoadResponse(this Load load)
//         {
//             return new LoadDtoResponse(
//                 load.LoadId,
//                 load.ShipperUserId,
//                 load.Shipper.AsShipperResponse(), // Transform User entity to ShipperDto
//                 load.Origin,
//                 load.Destination,
//                 load.PickupDate,
//                 load.DeliveryDate,
//                 load.Commodity,
//                 load.Weight,
//                 load.OfferAmount,
//                 load.LoadDetails,
//                 load.LoadStatus,
//                 load.CreatedAt,
//                 load.ModifiedAt,
//                 load.ModifiedBy
//             );
//         }

//         public static Load AsCreateLoad(this CreateLoadDto createLoadDto)
//         {
//             return new Load
//             {
//                 ShipperUserId = createLoadDto.ShipperUserId,
//                 Origin = createLoadDto.Origin,
//                 Destination = createLoadDto.Destination,
//                 PickupDate = createLoadDto.PickupDate,
//                 DeliveryDate = createLoadDto.DeliveryDate,
//                 Commodity = createLoadDto.Commodity,
//                 Weight = createLoadDto.Weight,
//                 OfferAmount = createLoadDto.OfferAmount,
//                 LoadDetails = createLoadDto.LoadDetails,
//                 LoadStatus = createLoadDto.LoadStatus,
//                 CreatedAt = createLoadDto.CreatedAt,
//                 Shipper = createLoadDto.CreatedBy.AsUser()
//             };
//         }

//         public static ShipperDtoResponse AsShipperResponse(this User user)
//         {
//             return new ShipperDtoResponse(
//                 UserId: user.UserId,
//                 Email: user.Email,
//                 FirstName: user.FirstName,
//                 MiddleName: user.MiddleName,
//                 LastName: user.LastName,
//                 Phone: user.Phone,
//                 UserType: user.UserType,
//                 BusinessProfile: user.BusinessProfile?.AsBusinessProfileDto()
//             );
//         }

//         public static Load AsLoad(this LoadDto loadDto)
//         {
//             return new Load
//             {
//                 LoadId = loadDto.LoadId,
//                 ShipperUserId = loadDto.ShipperUserId,
//                 Origin = loadDto.Origin,
//                 Destination = loadDto.Destination,
//                 PickupDate = loadDto.PickupDate,
//                 DeliveryDate = loadDto.DeliveryDate,
//                 Commodity = loadDto.Commodity,
//                 Weight = loadDto.Weight,
//                 OfferAmount = loadDto.OfferAmount,
//                 LoadDetails = loadDto.LoadDetails,
//                 LoadStatus = loadDto.LoadStatus,
//                 CreatedAt = (DateTime)loadDto.CreatedAt,
//                 ModifiedAt = loadDto.ModifiedAt,
//                 ModifiedBy = loadDto.ModifiedBy,
//                 Shipper = loadDto.CreatedBy.AsUser()
//             };
//         }
        
//         public static CarrierResponse AsCarrierResponse(this User user)
//         {
//             return new CarrierResponse(
//                 UserId: user.UserId,
//                 Email: user.Email,
//                 FirstName: user.FirstName,
//                 MiddleName: user.MiddleName,
//                 LastName: user.LastName,
//                 Phone: user.Phone,
//                 UserType: user.UserType,
//                 DOTNumber: user.BusinessProfile?.DOTNumber,
//                 MotorCarrierNumber: user.BusinessProfile?.MotorCarrierNumber,
//                 EquipmentType: user.BusinessProfile?.EquipmentType,
//                 AvailableCapacity: user.BusinessProfile?.AvailableCapacity,
//                 CompanyName: user.BusinessProfile?.CompanyName,
//                 VehicleTypes: user.BusinessProfile?.BusinessVehicleTypes?.Select(vt => vt.AsVehicleDto()).ToArray() ?? Array.Empty<VehicleTypeDto>(),
//                 CarrierRole: user.BusinessProfile?.CarrierRole ?? 0
//             );
//         }

//         public static UserDto AsUserDto(this User user)
//         {
//             return new UserDto(
//                 UserId: user.UserId,
//                 Email: user.Email,
//                 FirstName: user.FirstName,
//                 MiddleName: user.MiddleName,
//                 LastName: user.LastName,
//                 Phone: user.Phone,
//                 UserType: user.UserType,
//                 BusinessProfile: user.BusinessProfile?.AsBusinessProfileDto() 
//             );
//         }
//         public static BidDtoResponse AsBidResponse(this Bid bid)
//         {
//             return new BidDtoResponse(
//                 bid.Id,
//                 bid.LoadId,
//                 bid.CarrierId,
//                 bid.BidAmount,
//                 bid.BidStatus,
//                 bid.BiddingTime,
//                 bid.UpdatedAt,
//                 bid.UpdatedBy,
//                 bid.Load.AsLoadResponse(),     // Transform Load entity to LoadDto
//                 bid.Carrier.AsCarrierResponse()   // Transform User entity (acting as Carrier) to CarrierDto
//             );
//         }
//     }
// }
