namespace DemoCasino.Services.Shared.Interfaces;

public interface IRabbitMQConsumer<T>
{
    void Consume(string queueName, Action<T> handleMessage);
    void Consume(string queueName, Func<T, Task> handleMessage);
}
