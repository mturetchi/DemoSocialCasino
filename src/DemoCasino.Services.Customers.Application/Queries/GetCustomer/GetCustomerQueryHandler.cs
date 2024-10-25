using DemoCasino.Services.Customers.Application.Interfaces;
using DemoCasino.Services.Customers.Core.Constants;
using DemoCasino.Services.Customers.Core.ViewModels;
using DemoCasino.Services.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DemoCasino.Services.Customers.Application.Queries.GetCustomer;

class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerViewModel>
{
    private readonly ICustomersDbContext _dbContext;
    private readonly IFundsEventsPublisher _fundsEventsPublisher;
    private readonly IGetFundsResponseHandler _getFundsResponseHandler;
    private readonly ICacheService _cacheService;

    public GetCustomerQueryHandler(
        ICustomersDbContext dbContext,
        IFundsEventsPublisher fundsEventsPublisher,
        IGetFundsResponseHandler getFundsResponseHandler,
        ICacheService cacheService
    )
    {
        _dbContext = dbContext;
        _fundsEventsPublisher = fundsEventsPublisher;
        _getFundsResponseHandler = getFundsResponseHandler;
        _cacheService = cacheService;
    }

    public async Task<CustomerViewModel> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"{CustomerCacheKey.Customer}:{request.Id}";

        var cachedCustomer = await _cacheService.GetAsync<CustomerViewModel>(cacheKey);
        if (cachedCustomer != null)
            return cachedCustomer;

        var customer = await _dbContext
            .Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        var correlationId = _fundsEventsPublisher.GetFunds(request.Id);

        var funds = await _getFundsResponseHandler.WaitForResponse(request.Id, correlationId);

        var customerViewModel = new CustomerViewModel
        {
            Id = request.Id,
            Email = customer.Email,
            Funds = new CustomerFundsDTO
            {
                Amount = funds.Amount
            },
            Name = customer.Name,
        };

        await _cacheService.SetAsync(cacheKey, customerViewModel, TimeSpan.FromMinutes(5));

        return customerViewModel;
    }
}
