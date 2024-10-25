using DemoCasino.Services.Bets.Application.Interfaces;
using DemoCasino.Services.Bets.Core.Entities;
using DemoCasino.Services.Bets.Core.ViewModels;
using MediatR;

namespace DemoCasino.Services.Bets.Application.Commands.CreateBets;

class CreateBetCommandHandler : IRequestHandler<CreateBetCommand, BetViewModel>
{
    private readonly IBetsDbContext _dbContext;
    private readonly IBetsService _betsService;
    private readonly ICheckFundsResponseHandler _fundsResponseHandler;

    public CreateBetCommandHandler(
        IBetsDbContext dbContext,
        IBetsService betsService,
        ICheckFundsResponseHandler fundsResponseHandler
    )
    {
        _dbContext = dbContext;
        _betsService = betsService;
        _fundsResponseHandler = fundsResponseHandler;
    }

    public async Task<BetViewModel> Handle(CreateBetCommand request, CancellationToken cancellationToken)
    {
        var correlationId = _betsService.CheckFunds(request.UserId, request.Amount);

        var fundsResponse = await _fundsResponseHandler.WaitForResponse(request.UserId, correlationId);

        if (!fundsResponse.HasSufficientFunds)
            throw new Exception("Insufficient funds");

        var bet = new Bet
        {
            Amount = request.Amount,
            GameId = request.GameId,
            TransactionId = request.TransactionId,
            UserSessionId = request.UserId,
        };

        _dbContext.Bets.Add(bet);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _betsService.PlaceBet(request.UserId, request.Amount);

        return new BetViewModel
        {
            Id = bet.Id,
            Amount = bet.Amount,
            GameId = bet.GameId,
            TransactionId = bet.TransactionId,
            UserSessionId = bet.UserSessionId
        };
    }
}
