using DemoCasino.Services.Funds.Application.Interfaces;
using DemoCasino.Services.Funds.Core.Entities;
using MediatR;

namespace DemoCasino.Services.Funds.Application.Commands.InitializeFund;

class InitializeFundsCommandHandler : IRequestHandler<InitializeFundCommand>
{
    private readonly IFundsDbContext _dbContext;

    public InitializeFundsCommandHandler(IFundsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(InitializeFundCommand request, CancellationToken cancellationToken)
    {
        _dbContext.Funds.Add(new Fund
        {
            Amount = request.Amount,
            CustomerId = request.UserId
        });
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
