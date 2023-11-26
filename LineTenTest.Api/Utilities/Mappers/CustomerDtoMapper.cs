using LineTenTest.Api.Dtos;
using LineTenTest.Domain.Entities;

namespace LineTenTest.Api.Utilities.Mappers;

public class CustomerDtoMapper
{
    public static CustomerDto MapFrom(Customer customer)
    {
        return new CustomerDto()
        {
            Email = customer.Email,
            FirstName = customer.FirstName,
            Id = customer.Id,
            LastName = customer.LastName,
            Phone = customer.Phone
        };
    }
}