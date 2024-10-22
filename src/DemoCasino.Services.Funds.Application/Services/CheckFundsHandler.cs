using DemoCasino.Services.Funds.Application.Interfaces;
using DemoCasino.Services.Funds.Application.Queries.CheckFunds;
using DemoCasino.Services.Shared.Constants;
using DemoCasino.Services.Shared.Interfaces;
using DemoCasino.Services.Shared.Models.Funds;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DemoCasino.Services.Funds.Application.Services;

class CheckFundsHandler : ICheckFundsHandler
{
    private readonly IRabbitMQConsumer<CheckFundsRequest> _consumer;
    private readonly IRabbitMQProducer _producer;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<CheckFundsHandler> _logger;

    public CheckFundsHandler(
        IRabbitMQConsumer<CheckFundsRequest> consumer,
        IRabbitMQProducer producer,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<CheckFundsHandler> logger
    )
    {
        _consumer = consumer;
        _producer = producer;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _consumer.Consume(FundQueues.CheckFundsQueue, HandleCheckFundsRequest);
        _logger.LogInformation("Subscribed...");
    }

    private async Task HandleCheckFundsRequest(CheckFundsRequest request)
    {
        _logger.LogInformation($"HandleCheckFundsRequest, {request.UserId}, {request.Amount}");

        bool hasSufficientFunds = await CheckFunds(request.UserId, request.Amount);

        var response = new CheckFundsResponse
        {
            UserId = request.UserId,
            HasSufficientFunds = hasSufficientFunds
        };

        _logger.LogInformation($"Has Sufficient funds: {hasSufficientFunds}");

        _producer.Publish(response, FundQueues.CheckFundsResponseQueue);
    }

    private async Task<bool> CheckFunds(Guid userId, decimal amount)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(new CheckFundsQuery { Amount = amount, UserId = userId });
    }

    public void Subscribe()
    {
        _logger.LogInformation("Subscribe...");
    }
}
