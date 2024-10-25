namespace DemoCasino.Services.Bets.Application.Interfaces;

interface IBetsService
{
    Guid CheckFunds(Guid userId, decimal amount);
    void PlaceBet(Guid userId, decimal amount);
}
