using DemoCasino.Services.Funds.Application.Commands.InitializeFund;
using DemoCasino.Services.Funds.Application.Interfaces;
using DemoCasino.Services.Shared.Constants;
using DemoCasino.Services.Shared.Interfaces;
using DemoCasino.Services.Shared.Models.Funds;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace DemoCasino.Services.Funds.Application.Services;

class FundsInitializeHandler : IFundsInitializeHandler
{
    private readonly IRabbitMQConsumer<InitializeFundsRequest> _consumer;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<FundsInitializeHandler> _logger;

    public FundsInitializeHandler(
        IRabbitMQConsumer<InitializeFundsRequest> consumer,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<FundsInitializeHandler> logger
    )
    {
        _consumer = consumer;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _consumer.Consume(FundQueues.InitializeFundsQueue, HandleInitializeFundsRequest);
    }

    private async Task HandleInitializeFundsRequest(InitializeFundsRequest request)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new InitializeFundCommand { Amount = request.Amount, UserId = request.UserId });
    }

    public void Subscribe()
    {
        _logger.LogInformation("Subscribe...");
    }
}
