namespace DemoCasino.Services.Shared.Models.Funds;

public class GetFundsResponse
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
}