using DemoCasino.Services.Funds.Application.Interfaces;
using DemoCasino.Services.Funds.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoCasino.Services.Funds.Infrastructure;

class FundsDbContext : DbContext, IFundsDbContext
{
    public FundsDbContext(DbContextOptions<FundsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Fund> Funds { get; set; }
}
