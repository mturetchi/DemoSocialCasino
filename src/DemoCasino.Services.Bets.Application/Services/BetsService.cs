using DemoCasino.Services.Bets.Application.Interfaces;
using DemoCasino.Services.Shared.Constants;
using DemoCasino.Services.Shared.Interfaces;
using DemoCasino.Services.Shared.Models.Funds;
using Microsoft.Extensions.Logging;

namespace DemoCasino.Services.Bets.Application.Services;

class BetsService : IBetsService
{
    private readonly IRabbitMQProducer _producer;
    private readonly ILogger<BetsService> _logger;

    public BetsService(
        IRabbitMQProducer producer,
        ILogger<BetsService> logger    
    )
    {
        _producer = producer;
        _logger = logger;
    }

    public void CheckFunds(Guid userId, decimal amount)
    {
        _logger.LogInformation($"Check funds: {userId}, {amount}");

        var checkFundsRequest = new CheckFundsRequest
        {
            UserId = userId,
            Amount = amount
        };

        _producer.Publish(checkFundsRequest, FundQueues.CheckFundsQueue);
    }

    public void PlaceBet(Guid userId, decimal amount)
    {
        _logger.LogInformation($"Place bet: {userId}, {amount}");

        var checkFundsRequest = new PlaceBetRequest
        {
            UserId = userId,
            Amount = amount
        };

        _producer.Publish(checkFundsRequest, FundQueues.PlaceBetQueue);
    }
}
