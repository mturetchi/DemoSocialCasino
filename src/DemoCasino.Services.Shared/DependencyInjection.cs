using DemoCasino.Services.Shared.Interfaces;
using DemoCasino.Services.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RedLockNet.SERedis.Configuration;
using RedLockNet.SERedis;
using StackExchange.Redis;
using RedLockNet;

namespace DemoCasino.Services.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddShared(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(sp =>
        {
            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"],
                Port = int.Parse(configuration["RabbitMQ:Port"]),
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"]
            };

            return factory.CreateConnection();
        });

        services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();

        var redis = ConnectionMultiplexer.Connect($"{configuration["Redis:Host"]}:{configuration["Redis:Port"]}");
        services.AddSingleton<IConnectionMultiplexer>(redis);
        services.AddSingleton<IDistributedLockFactory>(sp =>
        {
            return RedLockFactory.Create(new List<RedLockMultiplexer>
            {
                new(redis)
            });
        });

        services.AddSingleton<ICacheService, RedisCacheService>();

        return services;
    }
}
