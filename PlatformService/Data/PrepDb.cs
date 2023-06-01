using PlatformService.Data;
using PlatformService.Models;

public static class PrepDb
{
  public static void PrepPoupulation(IApplicationBuilder app)
  {
    using (var serviceScope = app.ApplicationServices.CreateScope())
    {
      var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
      SeedData(context);
    }
  }

  private static void SeedData(AppDbContext context)
  {
    if (!context.Platforms.Any())
    {
      Console.WriteLine("--> Seeding Data...");
      context.AddRange(
        new Platform{Name ="Dot Net", Publisher="Microsoft", Cost="Free"},
        new Platform{Name ="SQL Server", Publisher="Microsoft", Cost="Free"},
        new Platform{Name ="Kubernetes", Publisher="Cloud Native Computing Foundation", Cost="Free"}
      );
      context.SaveChanges();
    }
    else
    {
      Console.WriteLine("--> We already have data");
    }
  }
}