using DemoCasino.Services.Customers.Application.Interfaces;
using DemoCasino.Services.Customers.Application.Services;
using DemoCasino.Services.Shared.Interfaces;
using DemoCasino.Services.Shared.Models.Funds;
using DemoCasino.Services.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DemoCasino.Services.Customers.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddSingleton<IFundsEventsPublisher, FundEventsPublisher>();
        services.AddSingleton<IGetFundsResponseHandler, GetFundsResponseHandler>();

        services.AddSingleton<IRabbitMQConsumer<GetFundsResponse>, RabbitMQConsumer<GetFundsResponse>>();

        return services;
    }
}
