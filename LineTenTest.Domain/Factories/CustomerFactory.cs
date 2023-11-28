using LineTenTest.Api.Controllers;

namespace LineTenTest.Domain.Factories;

public class CustomerFactory
{
    public static Entities.Customer Create(CreateCustomerRequest request)
    {
        return new Entities.Customer()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email
        };
    }
}