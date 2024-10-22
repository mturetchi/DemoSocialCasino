namespace DemoCasino.Services.Shared.Interfaces;

public interface IRabbitMQProducer
{
    void Publish<T>(T message, string queueName);
}
