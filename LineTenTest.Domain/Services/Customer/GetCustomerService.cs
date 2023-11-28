using LineTenTest.Domain.Exceptions;
using LineTenTest.SharedKernel;

namespace LineTenTest.Domain.Services.Customer;

public class GetCustomerService : IGetCustomerService
{
    private readonly IRepository<Entities.Customer> _repository;

    public GetCustomerService(IRepository<Entities.Customer> repository)
    {
        _repository = repository;
    }

    public async Task<Entities.Customer> GetAsync(int customerId)
    {
        throw new NotImplementedException();
    }
}