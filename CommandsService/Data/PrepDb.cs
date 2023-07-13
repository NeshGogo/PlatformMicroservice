using CommandsService.Data;
using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data;
public static class PrepDb
{
  public static void PrepPopulation(IApplicationBuilder app)
  {
    using (var serviceScope = app.ApplicationServices.CreateScope())
    {
      var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
      var platforms = grpcClient.ReturnsAllPlatforms();
      SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
    }
  }

  private static void SeedData(ICommandRepo repom, IEnumerable<Platform> platforms)
  {
    Console.WriteLine("--> Seeding new platforms...");
    foreach (var platform in platforms)
    {
      if(!repom.ExternalPlatformExists(platform.ExternalId)){
        repom.CreatePlatform(platform);
      }
      repom.SaveChanges();
    }
  }
}
