using DemoCasino.Services.Funds.Application.Commands.Withdraw;
using DemoCasino.Services.Funds.Application.Interfaces;
using DemoCasino.Services.Shared.Constants;
using DemoCasino.Services.Shared.Interfaces;
using DemoCasino.Services.Shared.Models.Funds;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace DemoCasino.Services.Funds.Application.Services;

class PlaceBetHandler : IPlaceBetHandler
{
    private readonly IRabbitMQConsumer<PlaceBetRequest> _consumer;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<PlaceBetHandler> _logger;

    public PlaceBetHandler(
        IRabbitMQConsumer<PlaceBetRequest> consumer,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<PlaceBetHandler> logger
    )
    {
        _consumer = consumer;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _consumer.Consume(FundQueues.PlaceBetQueue, HandlePlaceBetRequest);
    }

    private async Task HandlePlaceBetRequest(PlaceBetRequest request)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new WithdrawCommand { Amount = request.Amount, CustomerId = request.UserId });
    }

    public void Subscribe()
    {
        _logger.LogInformation("Subscribe...");
    }
}