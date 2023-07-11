using AutoMapper;
using PlatformService;
using PlatformService.DTOs;
using PlatformService.Models;

public class PlatformProfiles : Profile
{
  public PlatformProfiles()
  {
    CreateMap<Platform, PlatFormReadDTO>();
    CreateMap<PlatformCreateDTO, Platform>();
    CreateMap<PlatFormReadDTO, PlatformPublishDTO>();
    CreateMap<Platform, GrpcPlatformModel>()
      .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id));
  }
}