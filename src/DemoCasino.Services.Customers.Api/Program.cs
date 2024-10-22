using DemoCasino.Services.Customers.Infrastructure;
using MediatR;
using DemoCasino.Services.Customers.Application.Commands.CreateCustomer;
using DemoCasino.Services.Customers.Application;
using DemoCasino.Services.Customers.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using DemoCasino.Services.Shared;
using DemoCasino.Services.Customers.Application.Queries.GetCustomer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddShared(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapPost("/customers", async (CreateCustomerCommand request, IMediator mediator) =>
{
    return await mediator.Send(request);
});

app.MapGet("/customers/{id}", async (Guid id, IMediator mediator) =>
{
    return await mediator.Send(new GetCustomerQuery { Id = id });
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ICustomersDbContext>();
    await dbContext.Database.MigrateAsync();
    scope.ServiceProvider.GetRequiredService<IGetFundsResponseHandler>().Subscribe();
}

app.Run();
