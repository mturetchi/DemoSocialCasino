namespace DemoCasino.Services.Shared.Models.Funds;

public class PlaceBetRequest
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
}
