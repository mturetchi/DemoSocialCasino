using DemoCasino.Services.Customers.Application.Interfaces;
using DemoCasino.Services.Customers.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoCasino.Services.Customers.Infrastructure;

class CustomersDbContext : DbContext, ICustomersDbContext
{
    public CustomersDbContext(DbContextOptions<CustomersDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
}
