namespace LineTenTest.Api.Services.Customer;

public interface IGetCustomerService
{
    Task<Domain.Entities.Customer> GetAsync(int customerId);
}