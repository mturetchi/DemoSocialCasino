using DemoCasino.Services.Funds.Application.Interfaces;
using DemoCasino.Services.Funds.Core.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DemoCasino.Services.Funds.Application.Queries.GetFunds;

public class GetFundsQuery : IRequest<FundViewModel>
{
    public Guid CustomerId { get; set; }
}

class GetFundsQueryHandler : IRequestHandler<GetFundsQuery, FundViewModel>
{
    private readonly IFundsDbContext _dbContext;

    public GetFundsQueryHandler(IFundsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<FundViewModel> Handle(GetFundsQuery request, CancellationToken cancellationToken) =>
        _dbContext
            .Funds
            .AsNoTracking()
            .Select(f => new FundViewModel
            {
                CustomerId = f.CustomerId,
                Amount = f.Amount,
            }).FirstOrDefaultAsync(f => f.CustomerId == request.CustomerId, cancellationToken);
}
