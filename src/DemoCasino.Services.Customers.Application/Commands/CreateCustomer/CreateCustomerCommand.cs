using DemoCasino.Services.Customers.Application.Interfaces;
using DemoCasino.Services.Customers.Core.Entities;
using DemoCasino.Services.Customers.Core.ViewModels;
using MediatR;

namespace DemoCasino.Services.Customers.Application.Commands.CreateCustomer;

public class CreateCustomerCommand : IRequest<CustomerViewModel>
{
    public string Name { get; set; }
    public string Email { get; set; }
}

class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerViewModel>
{
    private readonly ICustomersDbContext _dbContext;
    private readonly IFundsEventsPublisher _fundEventsPublisher;

    public CreateCustomerCommandHandler(
        ICustomersDbContext dbContext,
        IFundsEventsPublisher fundEventsPublisher    
    )
    {
        _dbContext = dbContext;
        _fundEventsPublisher = fundEventsPublisher;
    }

    public async Task<CustomerViewModel> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email,
        };

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _fundEventsPublisher.InitializeFunds(customer.Id, 0);

        return new CustomerViewModel
        {
            Id = customer.Id,
            Email = customer.Email,
            Name = customer.Name
        };
    }
}
