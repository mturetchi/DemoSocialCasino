using DemoCasino.Services.Bets.Application.Interfaces;
using DemoCasino.Services.Bets.Core.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DemoCasino.Services.Bets.Application.Queries.GetBets;

public class GetBetsQuery : IRequest<List<BetViewModel>>
{
}

class GetBetsQueryHandler : IRequestHandler<GetBetsQuery, List<BetViewModel>>
{
    private readonly IBetsDbContext _dbContext;

    public GetBetsQueryHandler(IBetsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<BetViewModel>> Handle(GetBetsQuery request, CancellationToken cancellationToken) =>
        _dbContext
            .Bets
            .AsNoTracking()
            .Select(bet => new BetViewModel
            {
                Id = bet.Id,
                Amount = bet.Amount,
                GameId = bet.GameId,
                TransactionId = bet.TransactionId,
                UserSessionId = bet.UserSessionId
            })
            .ToListAsync(cancellationToken);
}
