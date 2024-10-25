using DemoCasino.Services.Bets.Application.Interfaces;
using DemoCasino.Services.Shared.Constants;
using DemoCasino.Services.Shared.Interfaces;
using DemoCasino.Services.Shared.Models.Funds;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DemoCasino.Services.Bets.Application.Services;

class CheckFundsResponseHandler : ICheckFundsResponseHandler
{
    private readonly ConcurrentDictionary<Guid, TaskCompletionSource<CheckFundsResponse>> _pendingRequests = new();
    private readonly ILogger<CheckFundsResponseHandler> _logger;

    public CheckFundsResponseHandler(
        IRabbitMQConsumer<CheckFundsResponse> consumer,
        ILogger<CheckFundsResponseHandler> logger
    )
    {
        consumer.Consume(FundQueues.CheckFundsResponseQueue, HandleFundsResponse);
        _logger = logger;
    }

    private void HandleFundsResponse(CheckFundsResponse response)
    {
        if (_pendingRequests.TryRemove(response.CorrelationId, out var tcs))
        {
            tcs.SetResult(response);
            _logger.LogInformation($"Funds response received for UserId: {response.UserId}, SufficientFunds: {response.HasSufficientFunds}, CorrelationId: {response.CorrelationId}");
        }
        else throw new Exception("Correlation not found");
    }

    public Task<CheckFundsResponse> WaitForResponse(Guid userId, Guid correlationId)
    {
        var tcs = new TaskCompletionSource<CheckFundsResponse>();

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
