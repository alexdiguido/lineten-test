using LineTenTest.Api.Dtos;
using LineTenTest.Domain.Entities;

namespace LineTenTest.Api.Utilities.Mappers;

public class ProductDtoMapper
{
    public static ProductDto MapFrom(Product product)
    {
        return new ProductDto()
        {
            Id = product.Id,
            Description = product.Description,
            Name = product.Name,
            SKU = product.SKU
        };
    }
}