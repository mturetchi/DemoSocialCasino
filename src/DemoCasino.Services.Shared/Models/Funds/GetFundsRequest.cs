﻿namespace DemoCasino.Services.Shared.Models.Funds;

public class GetFundsRequest
{
    public Guid UserId { get; set; }
    public Guid CorrelationId { get; set; }
}
