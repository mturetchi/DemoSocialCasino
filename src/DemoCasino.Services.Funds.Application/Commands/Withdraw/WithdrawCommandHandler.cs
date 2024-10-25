using DemoCasino.Services.Funds.Application.Interfaces;
using DemoCasino.Services.Funds.Core.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedLockNet;

namespace DemoCasino.Services.Funds.Application.Commands.Withdraw;

class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, FundViewModel>
{
    private readonly IFundsDbContext _dbContext;
    private readonly IDistributedLockFactory _lockFactory;
    private readonly ILogger<WithdrawCommandHandler> _logger;

    public WithdrawCommandHandler(
        IFundsDbContext dbContext,
        IDistributedLockFactory lockFactory,
        ILogger<WithdrawCommandHandler> logger
    )
    {
        _dbContext = dbContext;
        _lockFactory = lockFactory;
        _logger = logger;
    }

    public async Task<FundViewModel> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        var resource = $"funds:lock:{request.CustomerId}";

        const int maxAttempts = 10;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            using var redLock = await _lockFactory.CreateLockAsync(resource, TimeSpan.FromSeconds(60));
            if (redLock.IsAcquired)
            {
                _logger.LogInformation($"Withdraw, CustomerId: {request.CustomerId}, Amount: {request.Amount}");

                var fund = await _dbContext
                    .Funds
                    .FirstOrDefaultAsync(f => f.CustomerId == request.CustomerId, cancellationToken);

                if (fund.Amount - request.Amount < 0)
                    throw new Exception("Insufficient balance");

                _logger.LogInformation($"Existing fund, CustomerId: {request.CustomerId}, Amount {fund.Amount}");

                fund.Amount -= request.Amount;

                _logger.LogInformation($"After editting, CustomerId: {request.CustomerId}, Amount {fund.Amount}");

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new FundViewModel
                {
                    CustomerId = fund.CustomerId,
                    Amount = fund.Amount,
                };
            }

            attempts++;
            await Task.Delay(100);
        }

        throw new Exception("Unable to acquire lock after multiple attempts");
    }
}
