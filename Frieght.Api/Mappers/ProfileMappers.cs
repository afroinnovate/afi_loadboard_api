using AutoMapper;
using Frieght.Api.Dtos;
using Frieght.Api.Entities;

namespace Frieght.Api.Mappings
{
  public class ProfileMapper : Profile
  {
    public ProfileMapper()
    {
      // User mappings
      CreateMap<User, CarrierDto>()
          .ForMember(dest => dest.VehicleTypes, opt => opt.MapFrom(src => src.BusinessProfile.CarrierVehicles));
      
      CreateMap<User, ShipperDto>();
      CreateMap<User, UserDto>()
          .ForMember(dest => dest.BusinessProfile, opt => opt.MapFrom(src => src.BusinessProfile));
      CreateMap<CreateCarrierDto, User>();
      CreateMap<UpdateCarrierDto, User>();
      CreateMap<CreateUserDto, User>();
      CreateMap<ShipperDto, User>();
      CreateMap<CarrierDto, User>().ForMember(dest => dest.BusinessProfile, opt => opt.MapFrom(src => src));

      // BusinessProfile mappings
      CreateMap<BusinessProfile, BusinessProfileDto>();
      CreateMap<BusinessProfileDto, BusinessProfile>();

      // Vehicle mappings
      CreateMap<Vehicle, VehicleDto>();
      CreateMap<VehicleDto, Vehicle>();

      // VehicleType mappings
      CreateMap<VehicleType, VehicleTypeDto>();
      CreateMap<VehicleTypeDto, VehicleType>();

      // Load mappings
      CreateMap<Load, LoadDto>();
      CreateMap<Load, LoadDtoResponse>()
          .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Shipper)); // Map Shipper to CreatedBy
      CreateMap<LoadDtoResponse, Load>()
          .ForMember(dest => dest.Shipper, opt => opt.MapFrom(src => src.CreatedBy));
      CreateMap<CreateLoadDto, Load>()
          .ForMember(dest => dest.Shipper, opt => opt.MapFrom(src => src.CreatedBy));
      CreateMap<LoadDto, Load>();
      CreateMap<UpdateLoadDto, Load>()
        .ForMember(dest => dest.Shipper, opt => opt.Ignore()); // If the Shipper is not being updated, you can ignore it

      // Bid mappings
      CreateMap<Bid, BidDto>();
      CreateMap<CreateBidDto, Bid>();
      CreateMap<UpdateBidDto, Bid>();
      CreateMap<Bid, BidDtoResponse>()
          .ForMember(dest => dest.Load, opt => opt.MapFrom(src => src.Load))
          .ForMember(dest => dest.Carrier, opt => opt.MapFrom(src => src.Carrier));
      CreateMap<CreateBidDto, Bid>();

      // Additional mappings
      CreateMap<User, ShipperDtoResponse>()
          .ForMember(dest => dest.BusinessProfile, opt => opt.MapFrom(src => src.BusinessProfile));

      CreateMap<VehicleType, VehicleTypeDto>();

      CreateMap<Vehicle, VehicleDto>()
          .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.VehicleTypeId, opt => opt.MapFrom(src => src.VehicleTypeId))
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
          .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
          .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
          .ForMember(dest => dest.VIN, opt => opt.MapFrom(src => src.VIN))
          .ForMember(dest => dest.LicensePlate, opt => opt.MapFrom(src => src.LicensePlate))
          .ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Make))
          .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
          .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
          .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
          .ForMember(dest => dest.HasInsurance, opt => opt.MapFrom(src => src.HasInsurance))
          .ForMember(dest => dest.HasRegistration, opt => opt.MapFrom(src => src.HasRegistration))
          .ForMember(dest => dest.HasInspection, opt => opt.MapFrom(src => src.HasInspection));

      CreateMap<User, CarrierResponse>()
      .ForCtorParam("UserId", opt => opt.MapFrom(src => src.UserId))
      .ForCtorParam("Email", opt => opt.MapFrom(src => src.Email))
      .ForCtorParam("FirstName", opt => opt.MapFrom(src => src.FirstName))
      .ForCtorParam("MiddleName", opt => opt.MapFrom(src => src.MiddleName))
      .ForCtorParam("LastName", opt => opt.MapFrom(src => src.LastName))
      .ForCtorParam("Phone", opt => opt.MapFrom(src => src.Phone))
      .ForCtorParam("UserType", opt => opt.MapFrom(src => src.UserType))
      .ForCtorParam("DOTNumber", opt => opt.MapFrom(src => src.BusinessProfile.DOTNumber))
      .ForCtorParam("MotorCarrierNumber", opt => opt.MapFrom(src => src.BusinessProfile.MotorCarrierNumber))
      .ForCtorParam("EquipmentType", opt => opt.MapFrom(src => src.BusinessProfile.EquipmentType))
      .ForCtorParam("AvailableCapacity", opt => opt.MapFrom(src => src.BusinessProfile.AvailableCapacity))
      .ForCtorParam("CompanyName", opt => opt.MapFrom(src => src.BusinessProfile.CompanyName))
      .ForCtorParam("Vehicles", opt => opt.MapFrom(src => src.BusinessProfile.CarrierVehicles))
      .ForCtorParam("CarrierRole", opt => opt.MapFrom(src => src.BusinessProfile.CarrierRole));
    }
  }
}
