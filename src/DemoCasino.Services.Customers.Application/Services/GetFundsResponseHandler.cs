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
        if (_pendingRequests.TryRemove(response.UserId, out var tcs))
            tcs.SetResult(response);
    }

    public Task<GetFundsResponse> WaitForResponse(Guid userId)
    {
        var tcs = new TaskCompletionSource<GetFundsResponse>();
        _pendingRequests[userId] = tcs;
        return tcs.Task;
    }

    public void Subscribe()
    {
        _logger.LogInformation("Subscribe...");
    }
}
