using DemoCasino.Services.Bets.Application.Interfaces;
using DemoCasino.Services.Bets.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoCasino.Services.Bets.Infrastructure;

class BetsDbContext : DbContext, IBetsDbContext
{
    public BetsDbContext(DbContextOptions<BetsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Bet> Bets { get; set; }
}
