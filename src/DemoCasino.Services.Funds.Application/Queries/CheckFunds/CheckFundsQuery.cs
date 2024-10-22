using MediatR;

namespace DemoCasino.Services.Funds.Application.Queries.CheckFunds;

public class CheckFundsQuery : IRequest<bool>
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
}
