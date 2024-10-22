using DemoCasino.Services.Shared.Entities;

namespace DemoCasino.Services.Funds.Core.Entities;

public class Fund : BaseEntity
{
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
}
