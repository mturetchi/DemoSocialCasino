using DemoCasino.Services.Bets.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DemoCasino.Services.Bets.Application.Interfaces;

public interface IBetsDbContext
{
    DbSet<Bet> Bets { get; set; }
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
