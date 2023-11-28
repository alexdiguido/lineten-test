using LineTenTest.Api.Controllers;
using LineTenTest.SharedKernel.ApiModels;

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

    public static Entities.Customer Create(DeleteCustomerRequest request)
    {
        return new Entities.Customer()
        {
            Id = request.CustomerId,
        };
    }
}