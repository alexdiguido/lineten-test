using Ardalis.Specification;

namespace LineTenTest.Domain.Services.Customer;

public class GetCustomerSpecification : Specification<Entities.Customer>
{
    public GetCustomerSpecification(int customerId)
    {
        Query.Where(customer => customer.Id == customerId)
            .AsNoTracking();
    }
}