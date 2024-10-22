namespace DemoCasino.Services.Customers.Core.ViewModels;

public class CustomerViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public CustomerFundsDTO Funds { get; set; }
}
