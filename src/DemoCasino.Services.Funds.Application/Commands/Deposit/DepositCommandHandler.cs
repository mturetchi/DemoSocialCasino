using DemoCasino.Services.Funds.Application.Interfaces;
using DemoCasino.Services.Funds.Core.Entities;
using DemoCasino.Services.Funds.Core.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DemoCasino.Services.Funds.Application.Commands.Deposit;

class DepositCommandHandler : IRequestHandler<DepositCommand, FundViewModel>
{
    private readonly IFundsDbContext _dbContext;

    public DepositCommandHandler(IFundsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FundViewModel> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        var fund = await _dbContext
            .Funds
            .FirstOrDefaultAsync(f => f.CustomerId == request.CustomerId, cancellationToken);

        if (fund == null)
        {
            fund = new Fund
            {
                CustomerId = request.CustomerId,
                Amount = 0,
            };
            _dbContext.Funds.Add(fund);
        }

        fund.Amount += request.Amount;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new FundViewModel
        {
            CustomerId = fund.CustomerId,
            Amount = fund.Amount,
        };
    }
}
