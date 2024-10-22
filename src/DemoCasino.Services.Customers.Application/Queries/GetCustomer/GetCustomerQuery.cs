using DemoCasino.Services.Customers.Core.ViewModels;
using MediatR;

namespace DemoCasino.Services.Customers.Application.Queries.GetCustomer;

public class GetCustomerQuery : IRequest<CustomerViewModel>
{
    public Guid Id { get; set; }
}
