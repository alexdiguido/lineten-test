using Ardalis.Specification;

namespace LineTenTest.Domain.Services.Customer;

public class GetCustomerToUpdateSpecification : Specification<Entities.Customer>
{
    public GetCustomerToUpdateSpecification(int customerId)
    {
        Query.Where(customer => customer.Id == customerId);
    }
}