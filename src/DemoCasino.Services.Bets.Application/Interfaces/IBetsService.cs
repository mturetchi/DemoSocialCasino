namespace DemoCasino.Services.Bets.Application.Interfaces;

interface IBetsService
{
    void CheckFunds(Guid userId, decimal amount);
    void PlaceBet(Guid userId, decimal amount);
}
