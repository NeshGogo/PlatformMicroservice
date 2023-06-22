using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/c/platforms/{platformId:int}/[Controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
  private readonly ICommandRepo _repository;
  private readonly IMapper _mapper;

  public CommandsController(ICommandRepo repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  [HttpGet]
  public ActionResult<IEnumerable<CommandReadDto>> Get(int platformId)
  {
    Console.WriteLine($"--> geting  commands for platforms with id {platformId}");
    if (!_repository.PlatformExists(platformId))
      return NotFound();
    var commands = _repository.GetCommandsForPlatform(platformId).ToList();
    return _mapper.Map<List<CommandReadDto>>(commands);
  }

  [HttpGet("{commandId:int}", Name = "GetCommandForPlatform")]
  public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
  {
    Console.WriteLine($"--> geting  command with id {platformId} / {commandId}");
    if (!_repository.PlatformExists(platformId))
      return NotFound();
    var command = _repository.GetCommand(platformId, commandId);
    if (command == null) return NotFound();
    return _mapper.Map<CommandReadDto>(command);
  }

  [HttpPost]
  public ActionResult<CommandReadDto> PostCommand(int platformId, CommandCreateDto createDto)
  {
    Console.WriteLine($"--> Creating a command into platform with id {platformId}");
    if (!_repository.PlatformExists(platformId))
      return NotFound();
    var command = _mapper.Map<Command>(createDto);
    _repository.CreateCommand(platformId, command);
    _repository.SaveChanges();
    var commandDTO = _mapper.Map<CommandReadDto>(command);
    return CreatedAtRoute(nameof(GetCommandForPlatform),
      new { PlatformId = platformId, CommandId = commandDTO.Id }, commandDTO);
  }
}
