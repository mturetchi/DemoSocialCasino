using DemoCasino.Services.Bets.Core.ViewModels;
using MediatR;

namespace DemoCasino.Services.Bets.Application.Commands.CreateBets;

public class CreateBetCommand : IRequest<BetViewModel>
{
    public Guid GameId { get; set; }
    public decimal Amount { get; set; }
    public Guid UserId { get; set; }
    public Guid TransactionId { get; set; }
}
