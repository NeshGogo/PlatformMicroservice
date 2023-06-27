using System.Text.Json;
using System.Windows.Input;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.EventProcessing;
public class EventProcessor : IEventProcessor
{
  private readonly IServiceScopeFactory _scopeFactory;
  private readonly IMapper _mapper;

  public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
  {
    _scopeFactory = scopeFactory;
    _mapper = mapper;
  }
  public void ProcessEvent(string message)
  {
    var eventType = DetermineEvent(message);
    switch (eventType)
    {
      case EventType.PlatformPusblished:
        AddPlatform(message);
        break;
      default:
        break;
    }
  }

  private void AddPlatform(string platformPublishedMessage)
  {
    using (var scope = _scopeFactory.CreateScope())
    {
      var repo = scope.ServiceProvider.GetService<ICommandRepo>();
      var platformPublishDTO = JsonSerializer.Deserialize<PlatformPublishDto>(platformPublishedMessage);
      try
      {
        var platform = _mapper.Map<Platform>(platformPublishDTO);
        if (!repo.ExternalPlatformExists(platform.ExternalId))
        {
          repo.CreatePlatform(platform);
          repo.SaveChanges();
        }
        else
          Console.WriteLine("--> Platform already exisits...");

      }
      catch (Exception ex)
      {
        Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
      }

    }
  }

  private EventType DetermineEvent(string notificationMessage)
  {
    Console.WriteLine("--> Determining Event");
    var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

    switch (eventType.Event)
    {
      case "Platform_Published":
        Console.WriteLine("--> Platform publish event detected");
        return EventType.PlatformPusblished;
      default:
        Console.WriteLine("--> Could not determine the event type");
        return EventType.Undetermined;
    }
  }
}


enum EventType
{
  PlatformPusblished,
  Undetermined
}