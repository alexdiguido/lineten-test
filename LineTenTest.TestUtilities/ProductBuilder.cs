using LineTenTest.Domain.Entities;

namespace LineTenTest.TestUtilities;

public class ProductBuilder
{
    public static Product CreateDefault()
    {
        return new Product()
        {
            Description = $"description",
            Id = 1,
            SKU = "sku",
            Name = "name"
        };
    }
}