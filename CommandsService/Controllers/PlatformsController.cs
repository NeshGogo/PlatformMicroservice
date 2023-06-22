using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/c/[Controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
  private readonly ICommandRepo _repository;
  private readonly IMapper _mapper;

  public PlatformsController(ICommandRepo repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  [HttpGet]
  public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
  {
    Console.WriteLine("--> Getting Platforms form CommandsService");
    var platoforms = _repository.GetPlatforms().ToList();
    return _mapper.Map<List<PlatformReadDto>>(platoforms);
  }

  [HttpPost]
  public ActionResult TestConnection()
  {
    Console.WriteLine("--> Inbound POST # Command Service");
    return Ok();
  }
}