namespace DemoCasino.Services.Shared.Models.Funds;

public class CheckFundsRequest
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
}
