using LineTenTest.Api.Controllers;
using LineTenTest.Domain.Factories;
using LineTenTest.SharedKernel;

namespace LineTenTest.Domain.Services.Customer;

public class CreateCustomerService : ICreateCustomerService
{
    private readonly IRepository<Entities.Customer> _repository;

    public CreateCustomerService(IRepository<Entities.Customer> repository)
    {
        _repository = repository;
    }

    public async Task<Entities.Customer> CreateAsync(CreateCustomerRequest request)
    {
        var customerEntity = CustomerFactory.Create(request);
        var customerResult = await _repository.AddAsync(customerEntity);
        await _repository.SaveChangesAsync();
        return customerResult;
    }
}