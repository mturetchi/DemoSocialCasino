using DemoCasino.Services.Bets.Application;
using DemoCasino.Services.Bets.Application.Commands.CreateBets;
using DemoCasino.Services.Bets.Application.Interfaces;
using DemoCasino.Services.Bets.Application.Queries.GetBets;
using DemoCasino.Services.Bets.Infrastructure;
using DemoCasino.Services.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddShared(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapPost("/bets", async (CreateBetCommand request, IMediator mediator) =>
{
    return await mediator.Send(request);
});

app.MapGet("/bets", async (IMediator mediator) =>
{
    return await mediator.Send(new GetBetsQuery());
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IBetsDbContext>();
    await dbContext.Database.MigrateAsync();
    scope.ServiceProvider.GetRequiredService<ICheckFundsResponseHandler>().Subscribe();
}

app.Run();
