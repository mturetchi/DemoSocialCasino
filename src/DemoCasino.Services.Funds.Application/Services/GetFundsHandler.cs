using DemoCasino.Services.Funds.Application.Interfaces;
using DemoCasino.Services.Funds.Application.Queries.CheckFunds;
using DemoCasino.Services.Funds.Application.Queries.GetFunds;
using DemoCasino.Services.Funds.Core.ViewModels;
using DemoCasino.Services.Shared.Constants;
using DemoCasino.Services.Shared.Interfaces;
using DemoCasino.Services.Shared.Models.Funds;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DemoCasino.Services.Funds.Application.Services;

class GetFundsHandler : IGetFundsHandler
{
    private readonly ILogger<GetFundsHandler> _logger;
    private readonly IRabbitMQConsumer<GetFundsRequest> _consumer;
    private readonly IRabbitMQProducer _producer;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public GetFundsHandler(
        ILogger<GetFundsHandler> logger,
        IRabbitMQConsumer<GetFundsRequest> consumer,
        IRabbitMQProducer producer,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        _logger = logger;
        _consumer = consumer;
        _producer = producer;
        _serviceScopeFactory = serviceScopeFactory;
        _consumer.Consume(FundQueues.GetFundsQueue, HandleGetFundsRequest);
    }

    private async Task HandleGetFundsRequest(GetFundsRequest request)
    {
        var funds = await GetFunds(request.UserId);
        _producer.Publish(new GetFundsResponse { UserId = funds.CustomerId, Amount = funds.Amount}, FundQueues.GetFundsResponseQueue);
    }

    private async Task<FundViewModel> GetFunds(Guid userId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(new GetFundsQuery { CustomerId = userId });
    }

    public void Subscribe()
    {
        _logger.LogInformation("Subscribe");
    }
}
