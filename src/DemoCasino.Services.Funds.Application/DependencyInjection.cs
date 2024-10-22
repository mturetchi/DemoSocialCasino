using DemoCasino.Services.Funds.Application.Interfaces;
using DemoCasino.Services.Funds.Application.Services;
using DemoCasino.Services.Shared.Interfaces;
using DemoCasino.Services.Shared.Models.Funds;
using DemoCasino.Services.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DemoCasino.Services.Funds.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddSingleton<ICheckFundsHandler, CheckFundsHandler>();
        services.AddSingleton<IFundsInitializeHandler, FundsInitializeHandler>();
        services.AddSingleton<IPlaceBetHandler, PlaceBetHandler>();
        services.AddSingleton<IGetFundsHandler, GetFundsHandler>();

        services.AddSingleton<IRabbitMQConsumer<CheckFundsRequest>, RabbitMQConsumer<CheckFundsRequest>>();
        services.AddSingleton<IRabbitMQConsumer<InitializeFundsRequest>, RabbitMQConsumer<InitializeFundsRequest>>();
        services.AddSingleton<IRabbitMQConsumer<PlaceBetRequest>, RabbitMQConsumer<PlaceBetRequest>>();
        services.AddSingleton<IRabbitMQConsumer<GetFundsRequest>, RabbitMQConsumer<GetFundsRequest>>();

        return services;
    }
}
