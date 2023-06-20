using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Models;

public static class PrepDb
{
  public static void PrepPoupulation(IApplicationBuilder app, bool isProd)
  {
    using (var serviceScope = app.ApplicationServices.CreateScope())
    {
      var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
      SeedData(context, isProd);
    }
  }

  private static void SeedData(AppDbContext context, bool isProd)
  {
    if (isProd)
    {
      try
      {
        Console.WriteLine("--> Attempting to apply migrations...");
        context.Database.Migrate();
      }
      catch (Exception ex)
      {
        Console.WriteLine($"--> Could not run migrations. Error: {ex.Message}");
      }
    }

    if (!context.Platforms.Any())
    {
      Console.WriteLine("--> Seeding Data...");
      context.AddRange(
        new Platform { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
        new Platform { Name = "SQL Server", Publisher = "Microsoft", Cost = "Free" },
        new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
      );
      context.SaveChanges();
    }
    else
    {
      Console.WriteLine("--> We already have data");
    }
  }
}