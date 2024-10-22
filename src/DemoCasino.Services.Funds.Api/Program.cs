using DemoCasino.Services.Funds.Application;
using DemoCasino.Services.Funds.Application.Commands.Deposit;
using DemoCasino.Services.Funds.Application.Commands.Withdraw;
using DemoCasino.Services.Funds.Application.Interfaces;
using DemoCasino.Services.Funds.Infrastructure;
using DemoCasino.Services.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddShared(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapPost("/funds/deposit", async (DepositCommand request, IMediator mediator) =>
{
    return await mediator.Send(request);
});

app.MapPost("/funds/withdraw", async (WithdrawCommand request, IMediator mediator) =>
{
    return await mediator.Send(request);
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IFundsDbContext>();
    await dbContext.Database.MigrateAsync();

    scope.ServiceProvider.GetRequiredService<ICheckFundsHandler>().Subscribe();
    scope.ServiceProvider.GetRequiredService<IFundsInitializeHandler>().Subscribe();
    scope.ServiceProvider.GetRequiredService<IPlaceBetHandler>().Subscribe();
    scope.ServiceProvider.GetRequiredService<IGetFundsHandler>().Subscribe();
}

app.Run();

