using LineTenTest.Domain.Exceptions;
using LineTenTest.SharedKernel;
using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Domain.Services.Customer;

public class DeleteCustomerService : IDeleteCustomerService
{
    private readonly IRepository<Entities.Customer> _repository;

    public DeleteCustomerService(IRepository<Entities.Customer> repository)
    {
        _repository = repository;
    }
    public async Task DeleteAsync(DeleteCustomerRequest request)
    {
        var customer = await _repository.FirstOrDefaultAsync(new GetCustomerToUpdateSpecification(request.CustomerId));
        if (customer == null)
        {
            throw new NotFoundException("Customer not found");
        }
        await _repository.DeleteAsync(customer);
        await _repository.SaveChangesAsync();
    }
}