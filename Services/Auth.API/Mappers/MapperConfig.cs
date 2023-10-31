using Auth.API.Dtos;
using Auth.API.Models;
using AutoMapper;

namespace Auth.API.Mappers
{
  public class MapperConfig
  {
    public static MapperConfiguration RegisterMapping()
    {
        var MappingConfig =  new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ApplicationUser, UserDto>();
            cfg.CreateMap<UserDto, ApplicationUser>();
        });
        return MappingConfig;
    }
  } 
}