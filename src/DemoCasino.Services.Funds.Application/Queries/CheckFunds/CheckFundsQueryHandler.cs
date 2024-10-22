using DemoCasino.Services.Funds.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DemoCasino.Services.Funds.Application.Queries.CheckFunds;

class CheckFundsQueryHandler : IRequestHandler<CheckFundsQuery, bool>
{
    private readonly IFundsDbContext _dbContext;

    public CheckFundsQueryHandler(IFundsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(CheckFundsQuery request, CancellationToken cancellationToken)
    {
        var fund = await _dbContext
            .Funds
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.CustomerId == request.UserId);

        return fund.Amount - request.Amount >= 0;
    }
}
