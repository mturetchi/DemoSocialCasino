using DemoCasino.Services.Funds.Application.Interfaces;
using DemoCasino.Services.Funds.Core.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DemoCasino.Services.Funds.Application.Commands.Withdraw;

class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, FundViewModel>
{
    private readonly IFundsDbContext _dbContext;

    public WithdrawCommandHandler(IFundsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FundViewModel> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        var fund = await _dbContext
            .Funds
            .FirstOrDefaultAsync(f => f.CustomerId == request.CustomerId, cancellationToken);

        if (fund.Amount - request.Amount < 0)
            throw new Exception("Insufficient balance");

        fund.Amount -= request.Amount;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new FundViewModel
        {
            CustomerId = fund.CustomerId,
            Amount = fund.Amount,
        };
    }
}
