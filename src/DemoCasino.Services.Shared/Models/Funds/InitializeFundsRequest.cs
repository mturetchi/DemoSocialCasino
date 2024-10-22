namespace DemoCasino.Services.Shared.Models.Funds;

public class InitializeFundsRequest
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
}
