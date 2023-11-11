using Auth.API.Dtos;
using Auth.API.Models;
using AutoMapper;

namespace Auth.API.Mappers
{
    public class MapperConfig
    {
        public static MapperConfiguration RegisterMapping()
        {
            var MappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDto, ApplicationUser>()
                    .ForMember(dest => dest.DateRegistered, opt => opt.Ignore())
                    .ForMember(dest => dest.DateLastLoggedIn, opt => opt.Ignore())
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
                    .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
                    .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                    .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                    .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                    .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                    .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                    .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
                    .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
                    .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
                    .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
                    // ... add other properties to ignore
                    .ReverseMap();
            });
            return MappingConfig;
        }
    }
}
