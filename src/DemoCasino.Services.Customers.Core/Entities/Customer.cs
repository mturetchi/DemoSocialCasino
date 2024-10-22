using DemoCasino.Services.Shared.Entities;

namespace DemoCasino.Services.Customers.Core.Entities;

public class Customer : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
}
