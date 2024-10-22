using DemoCasino.Services.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace DemoCasino.Services.Shared.Services;

public class RabbitMQProducer : IRabbitMQProducer
{
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMQProducer> _logger;

    public RabbitMQProducer(
        IConnection connection,
        ILogger<RabbitMQProducer> logger
    )
    {
        _connection = connection;
        _logger = logger;
    }

    public void Publish<T>(T message, string queueName)
    {
        using var channel = _connection.CreateModel();
        var queue = channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        _logger.LogInformation(JsonSerializer.Serialize(queue));

        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
    }
}