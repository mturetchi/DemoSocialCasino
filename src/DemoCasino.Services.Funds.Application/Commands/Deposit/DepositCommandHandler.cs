using DemoCasino.Services.Funds.Application.Interfaces;
using DemoCasino.Services.Funds.Core.Entities;
using DemoCasino.Services.Funds.Core.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RedLockNet;

namespace DemoCasino.Services.Funds.Application.Commands.Deposit;

class DepositCommandHandler : IRequestHandler<DepositCommand, FundViewModel>
{
    private readonly IFundsDbContext _dbContext;
    private readonly IDistributedLockFactory _lockFactory;

    public DepositCommandHandler(
        IFundsDbContext dbContext,
        IDistributedLockFactory lockFactory
    )
    {
        _dbContext = dbContext;
        _lockFactory = lockFactory;
    }

    public async Task<FundViewModel> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        var resource = $"funds:lock:{request.CustomerId}";

        const int maxAttempts = 10;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            using var redLock = await _lockFactory.CreateLockAsync(resource, TimeSpan.FromSeconds(60));
            if (redLock.IsAcquired)
            {
                var fund = await _dbContext
                    .Funds
                    .FirstOrDefaultAsync(f => f.CustomerId == request.CustomerId, cancellationToken);

                fund.Amount += request.Amount;

                await _dbContext.SaveChangesAsync(cancellationToken);
                
                return new FundViewModel
                {
                    CustomerId = fund.CustomerId,
                    Amount = fund.Amount,
                };
            }

            attempts++;
            await Task.Delay(100); // Delay before the next attempt
        }

        throw new Exception("Unable to acquire lock after multiple attempts");
    }
}
