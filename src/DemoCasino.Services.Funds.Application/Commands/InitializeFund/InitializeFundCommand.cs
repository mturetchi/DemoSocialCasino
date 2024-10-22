using MediatR;

namespace DemoCasino.Services.Funds.Application.Commands.InitializeFund;

public class InitializeFundCommand : IRequest
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
}
