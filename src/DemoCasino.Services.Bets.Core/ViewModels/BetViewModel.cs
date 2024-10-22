namespace DemoCasino.Services.Bets.Core.ViewModels;

public class BetViewModel
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public decimal Amount { get; set; }
    public Guid UserSessionId { get; set; }
    public Guid TransactionId { get; set; }
}
