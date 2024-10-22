using DemoCasino.Services.Shared.Interfaces;
using DemoCasino.Services.Shared.Models.Funds;

namespace DemoCasino.Services.Customers.Application.Interfaces;

public interface IGetFundsResponseHandler : IEventSubscriber
{
    Task<GetFundsResponse> WaitForResponse(Guid userId);
}
