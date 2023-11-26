using LineTenTest.Domain.Entities;

namespace LineTenTest.TestUtilities;

public class CustomerBuilder
{
    public static Customer CreateDefault()
    {
        return new Customer()
        {
            Email = "email",
            Id = 2,
            FirstName = "FirstName",
            LastName = "LastName",
            Phone = "Phone"
        };
    }
}