using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
  private readonly IPlatformRepo _repository;
  private readonly IMapper _mapper;
  private readonly ICommandDataClient _commandDataClient;
  private readonly IMessageBusClient _messageBusClient;

  public PlatformsController(
    IPlatformRepo repository,
    IMapper mapper,
    ICommandDataClient commandDataClient,
    IMessageBusClient messageBusClient
  )
  {
    _repository = repository;
    _mapper = mapper;
    _commandDataClient = commandDataClient;
    _messageBusClient = messageBusClient;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<PlatFormReadDTO>>> GetPlatForm()
  {
    Console.WriteLine("--> Getting Platforms...");
    var results = await _repository.GetAllPlatformsAsync();
    var dtos = _mapper.Map<List<PlatFormReadDTO>>(results); ;
    return dtos;
  }

  [HttpGet("{id:int}", Name = "GetPlatformById")]
  public async Task<ActionResult<PlatFormReadDTO>> GetPlatformById(int id)
  {
    var platform = await _repository.GetPlatformByIdAsync(id);
    if (platform == null)
      return NotFound();
    return _mapper.Map<PlatFormReadDTO>(platform);
  }

  [HttpPost]
  public async Task<ActionResult> CreatePlatform([FromBody] PlatformCreateDTO createDTO)
  {
    var platform = _mapper.Map<Platform>(createDTO);
    await _repository.CreatePlatForm(platform);
    await _repository.SaveChangesAsync();

    var dto = _mapper.Map<PlatFormReadDTO>(platform);
    // Send sync message
    try
    {
      await _commandDataClient.SendPlatformToCommand(dto);
    }
    catch (System.Exception ex)
    {
      Console.WriteLine($"--> Could not send platform Synchronously: {ex.Message}");
    }

    // Send async message
    try
    {
      var platformPublishDTO = _mapper.Map<PlatformPublishDTO>(dto);
      platformPublishDTO.Event = "Platform_Published";
      _messageBusClient.PublishNewPlatform(platformPublishDTO);
    }
    catch (System.Exception ex)
    {
      Console.WriteLine($"--> Could not send platform Asynchronously: {ex.Message}");
    }
    return CreatedAtRoute(nameof(GetPlatformById), new { Id = dto.Id }, dto);
  }
}