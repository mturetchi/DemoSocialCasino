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
        if (_pendingRequests.TryRemove(response.UserId, out var tcs))
            tcs.SetResult(response);
    }

    public Task<CheckFundsResponse> WaitForResponse(Guid userId)
    {
        var tcs = new TaskCompletionSource<CheckFundsResponse>();
        _pendingRequests[userId] = tcs;
        return tcs.Task;
    }

    public void Subscribe()
    {
        _logger.LogInformation("Subscribe...");
    }
}
