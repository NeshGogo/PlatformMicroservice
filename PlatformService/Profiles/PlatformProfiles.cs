using AutoMapper;
using PlatformService.DTOs;
using PlatformService.Models;

public class PlatformProfiles : Profile
{
  public PlatformProfiles()
  {
    CreateMap<Platform, PlatFormReadDTO>();
    CreateMap<PlatformCreateDTO, Platform>();
  }
}