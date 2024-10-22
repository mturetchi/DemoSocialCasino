using DemoCasino.Services.Funds.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DemoCasino.Services.Funds.Application.Interfaces;

public interface IFundsDbContext
{
    DbSet<Fund> Funds { get; set; }
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
