using System.Text;
using System.Text.Json;
using PlatformService.DTOs;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
  private readonly IConfiguration _configuration;
  private readonly IConnection _connection;
  private readonly IModel _channel;

  public MessageBusClient(IConfiguration configuration)
  {
    _configuration = configuration;
    var factory = new ConnectionFactory()
    {
      HostName = _configuration.GetSection("RabbitMQ").GetValue<string>("Host"),
      Port = _configuration.GetSection("RabbitMQ").GetValue<int>("Port"),
    };
    try
    {
      _connection = factory.CreateConnection();
      _channel = _connection.CreateModel();

      _channel.ExchangeDeclare(
          exchange: _configuration.GetSection("RabbitMQ").GetValue<string>("Exchange"),
          type: ExchangeType.Fanout);

      _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
      Console.WriteLine("--> connected to Message Bus.");
    }
    catch (System.Exception ex)
    {
      Console.WriteLine($"--> Could not connect to the message bus: {ex.Message}");
    }
  }
  public void PublishNewPlatform(PlatformPublishDTO platformPublishDTO)
  {
    var message = JsonSerializer.Serialize(platformPublishDTO);
    if (_connection.IsOpen)
    {
      Console.WriteLine("--> RabbitMQ connection Open, Sending message...");
      SendMessage(message);
    }
    else
    {
      Console.WriteLine("--> RabbitMQ connection closed, not sening");
    }
  }

  private void SendMessage(string message)
  {
    var body = Encoding.UTF8.GetBytes(message);
    _channel.BasicPublish(
      exchange: _configuration.GetSection("RabbitMQ").GetValue<string>("Exchange"),
      routingKey: "",
      basicProperties: null,
      body: body);
    Console.WriteLine($"--> We have sent {message}");
  }

  public void Dispose(){
    Console.WriteLine("--> Message Bus dispose");
    if (_channel.IsOpen)
    {
      _channel.Close();
      _connection.Close();
    }
  }

  private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
  {
    Console.WriteLine($"--> RabbitMQ connection shutdown");
  }
}