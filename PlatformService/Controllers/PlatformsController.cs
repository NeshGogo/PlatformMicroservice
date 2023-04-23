using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
  private readonly IPlatformRepo _repository;
  private readonly IMapper _mapper;

  public PlatformsController(IPlatformRepo repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
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
    return CreatedAtRoute(nameof(GetPlatformById), new {Id = dto.Id}, dto);
  }
}