using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Models;

public class PlatformRepo : IPlatformRepo
{
  private AppDbContext _context;

  public PlatformRepo(AppDbContext context)
  {
    _context = context;
  }
  public async Task CreatePlatForm(Platform platform)
  {
    if (platform == null)
      throw new ArgumentNullException(nameof(platform));

    await _context.Platforms.AddAsync(platform);
  }

  public async Task<IEnumerable<Platform>> GetAllPlatformsAsync()
  {
    return await _context.Platforms.ToListAsync();
  }

  public async Task<Platform> GetPlatformByIdAsync(int id)
  {
    return await _context.Platforms.FirstOrDefaultAsync(p => p.Id == id);
  }

  public async Task<bool> SaveChangesAsync()
  {
    return (await _context.SaveChangesAsync() >= 0);
  }
}