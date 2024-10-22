using DemoCasino.Services.Shared.Interfaces;
using DemoCasino.Services.Shared.Models.Funds;

namespace DemoCasino.Services.Bets.Application.Interfaces;

public interface ICheckFundsResponseHandler : IEventSubscriber
{
    Task<CheckFundsResponse> WaitForResponse(Guid userId);
}
