using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Domain.Services.Customer;

public interface IDeleteCustomerService
{
    Task DeleteAsync(DeleteCustomerRequest request);
}