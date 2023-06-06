using System.Text;
using System.Text.Json;
using PlatformService.DTOs;

namespace PlatformService.SyncDataServices.Http;

public class HttpCommandDataClient : ICommandDataClient
{
  private readonly HttpClient _httpClient;
  private readonly IConfiguration _config;

  public HttpCommandDataClient(HttpClient httpClient, IConfiguration config)
  {
    _httpClient = httpClient;
    _config = config;
  }

  public async Task SendPlatformToCommand(PlatFormReadDTO plat)
  {
    var httpContent = new StringContent(
      JsonSerializer.Serialize(plat), 
      Encoding.UTF8,
      "application/json");
    var url = _config.GetSection("Services").GetValue<string>("CommandService");
    var response = await _httpClient.PostAsync(url + "/Platforms", httpContent);

    if(response.IsSuccessStatusCode){
      Console.WriteLine("--> Sync POST to CommandsService was OK!");
    } else
    {
      Console.WriteLine("--> Sync POST to CommandsService was NOT OK!");
    }
  }
}