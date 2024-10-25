using DemoCasino.Services.Customers.Application.Interfaces;
using DemoCasino.Services.Shared.Constants;
using DemoCasino.Services.Shared.Interfaces;
using DemoCasino.Services.Shared.Models.Funds;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DemoCasino.Services.Customers.Application.Services;

class GetFundsResponseHandler : IGetFundsResponseHandler
{
    private readonly ConcurrentDictionary<Guid, TaskCompletionSource<GetFundsResponse>> _pendingRequests = new();
    private readonly ILogger<GetFundsResponseHandler> _logger;

    public GetFundsResponseHandler(
        IRabbitMQConsumer<GetFundsResponse> consumer,
        ILogger<GetFundsResponseHandler> logger
    )
    {
        consumer.Consume(FundQueues.GetFundsResponseQueue, HandleGetFundsResponse);
        _logger = logger;
    }

    private void HandleGetFundsResponse(GetFundsResponse response)
    {
        if (_pendingRequests.TryRemove(response.CorrelationId, out var tcs))
        {
            tcs.SetResult(response);
            _logger.LogInformation($"Funds response received for UserId: {response.UserId}, Balance: {response.Amount}, CorrelationId: {response.CorrelationId}");
        }
        else throw new Exception("Correlation not found");
    }

    public Task<GetFundsResponse> WaitForResponse(Guid userId, Guid correlationId)
    {
        var tcs = new TaskCompletionSource<GetFundsResponse>();

        _pendingRequests[correlationId] = tcs;

        return tcs.Task.ContinueWith(task =>
        {
            _pendingRequests.TryRemove(correlationId, out _);
            return task.Result;
        });
    }

    public void Subscribe()
    {
    }
}
