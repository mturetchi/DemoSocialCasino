namespace DemoCasino.Services.Shared.Models.Funds;

public class CheckFundsResponse
{
    public Guid UserId { get; set; }
    public bool HasSufficientFunds { get; set; }
    public Guid CorrelationId { get; set; }
}