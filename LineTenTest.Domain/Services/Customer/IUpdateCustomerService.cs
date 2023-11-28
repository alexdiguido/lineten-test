using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Domain.Services.Customer;

public interface IUpdateCustomerService
{
    Task<Entities.Customer> UpdateAsync(UpdateCustomerRequest request);
}