using LineTenTest.Api.Controllers;

namespace LineTenTest.Domain.Services.Customer;

public interface ICreateCustomerService
{
    Task<Domain.Entities.Customer> CreateAsync(CreateCustomerRequest request);
}