using CommandsService.Models;

namespace CommandsService.Data;
public class CommandRepo : ICommandRepo
{
  private readonly AppDbContext _context;

  public CommandRepo(AppDbContext context)
  {
    _context = context;
  }
  public void CreateCommand(int platformId, Command command)
  {
    if (command == null)
      throw new ArgumentNullException(nameof(command));
    command.PlatformId = platformId;
    _context.Commands.Add(command);
  }

  public void CreatePlatform(Platform platform)
  {
    if (platform == null)
      throw new ArgumentNullException(nameof(platform));

    _context.Platforms.Add(platform);
  }

    public bool ExternalPlatformExists(int externalPlatformId)
    {
      return _context.Platforms.Any(p => p.ExternalId == externalPlatformId);
    }

    public Command GetCommand(int platformId, int commandId)
  {
    return _context.Commands.FirstOrDefault(p => p.Id == commandId && p.PlatformId == platformId);
  }

  public IEnumerable<Command> GetCommandsForPlatform(int platformId)
  {
    return _context.Commands
      .Where(p => p.PlatformId == platformId)
      .OrderBy(p => p.Platform.Name);
  }

  public IEnumerable<Platform> GetPlatforms()
  {
    return _context.Platforms;
  }

  public bool PlatformExists(int platformId)
  {
    return _context.Platforms.Any(p => p.Id == platformId);
  }

  public bool SaveChanges()
  {
    return _context.SaveChanges() >= 0;
  }
}
