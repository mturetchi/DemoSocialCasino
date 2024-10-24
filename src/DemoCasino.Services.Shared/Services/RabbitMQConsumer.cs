using DemoCasino.Services.Shared.Interfaces;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace DemoCasino.Services.Shared.Services;

public class RabbitMQConsumer<T> : IRabbitMQConsumer<T>
{
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMQConsumer<T>> _logger;

    public RabbitMQConsumer(
        IConnection connection,
        ILogger<RabbitMQConsumer<T>> logger
    )
    {
        _connection = connection;
        _logger = logger;
    }

    public void Consume(string queueName, Action<T> handleMessage)
    {
        var channel = _connection.CreateModel();
        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var jsonMessage = Encoding.UTF8.GetString(body);

            try
            {
                var message = JsonSerializer.Deserialize<T>(jsonMessage);
                handleMessage(message);
                channel.BasicAck(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing message: {jsonMessage}");
                channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
    }

    public void Consume(string queueName, Func<T, Task> handleMessage)
    {
        var channel = _connection.CreateModel();
        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var jsonMessage = Encoding.UTF8.GetString(body);
            _logger.LogInformation(jsonMessage);
            try
            {
                var message = JsonSerializer.Deserialize<T>(jsonMessage);
                await handleMessage(message);
                channel.BasicAck(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing message");
                channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
            }
        };
        channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
    }
}