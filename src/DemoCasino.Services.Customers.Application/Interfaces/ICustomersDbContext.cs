using DemoCasino.Services.Customers.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DemoCasino.Services.Customers.Application.Interfaces;

public interface ICustomersDbContext
{
    DbSet<Customer> Customers { get; set; }
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
