using DemoCasino.Services.Bets.Application.Interfaces;
using DemoCasino.Services.Bets.Application.Services;
using DemoCasino.Services.Shared.Interfaces;
using DemoCasino.Services.Shared.Models.Funds;
using DemoCasino.Services.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DemoCasino.Services.Bets.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddSingleton<IBetsService, BetsService>();
        services.AddSingleton<ICheckFundsResponseHandler, CheckFundsResponseHandler>();
        services.AddSingleton<IRabbitMQConsumer<CheckFundsResponse>, RabbitMQConsumer<CheckFundsResponse>>();
        services.AddSingleton<IRabbitMQConsumer<CheckFundsResponse>, RabbitMQConsumer<CheckFundsResponse>>();

        return services;
    }
}
