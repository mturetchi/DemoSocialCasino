namespace DemoCasino.Services.Customers.Application.Interfaces;

interface IFundsEventsPublisher
{
    void InitializeFunds(Guid userId, decimal amount);

    Guid GetFunds(Guid userId);
}
