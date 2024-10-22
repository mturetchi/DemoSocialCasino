using DemoCasino.Services.Customers.Application.Interfaces;
using DemoCasino.Services.Shared.Constants;
using DemoCasino.Services.Shared.Interfaces;
using DemoCasino.Services.Shared.Models.Funds;

namespace DemoCasino.Services.Customers.Application.Services;

class FundEventsPublisher : IFundsEventsPublisher
{
    private readonly IRabbitMQProducer _producer;

    public FundEventsPublisher(IRabbitMQProducer producer)
    {
        _producer = producer;
    }

    public void InitializeFunds(Guid userId, decimal amount)
    {
        _producer.Publish(new InitializeFundsRequest
        {
            UserId = userId,
            Amount = amount
        }, FundQueues.InitializeFundsQueue);
    }

    public void GetFunds(Guid userId)
    {
        _producer.Publish(new GetFundsRequest
        {
            UserId = userId,
        }, FundQueues.GetFundsQueue);
    }
}
