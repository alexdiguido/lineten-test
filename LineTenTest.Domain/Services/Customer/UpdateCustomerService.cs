using LineTenTest.Domain.Exceptions;
using LineTenTest.Domain.Factories;
using LineTenTest.SharedKernel;
using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Domain.Services.Customer;

public class UpdateCustomerService : IUpdateCustomerService
{
    private readonly IRepository<Entities.Customer> _repository;

    public UpdateCustomerService(IRepository<Entities.Customer> repository)
    {
        _repository = repository;
    }

    public async Task<Entities.Customer> UpdateAsync(UpdateCustomerRequest request)
    {
        var customer = await _repository.FirstOrDefaultAsync(new GetCustomerToUpdateSpecification(request.CustomerId));

        if (customer == null)
        {
            throw new EntityNotFoundException("customer not found");
        }

        customer.FirstName = request.FirstName;
        customer.LastName = request.LastName;
        customer.Email = request.Email;
        customer.Phone = request.Phone;

        await _repository.SaveChangesAsync();

        return customer;
    }
}