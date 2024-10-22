using DemoCasino.Services.Shared.Entities;

namespace DemoCasino.Services.Bets.Core.Entities;

public class Bet : BaseEntity
{
    public Guid GameId { get; set; }
    public decimal Amount { get; set; }
    public Guid UserSessionId { get; set; }
    public Guid TransactionId { get; set; }
}
