using DemoCasino.Services.Funds.Core.ViewModels;
using MediatR;

namespace DemoCasino.Services.Funds.Application.Commands.Deposit;

public class DepositCommand : IRequest<FundViewModel>
{
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
}
