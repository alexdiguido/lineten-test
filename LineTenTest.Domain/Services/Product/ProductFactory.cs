using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Domain.Services.Product;

public class ProductFactory
{
    public static Entities.Product CreateProduct(CreateProductRequest request)
    {
        return new Entities.Product()
        {
            Description = request.Description,
            Name = request.Name,
            SKU = request.Sku
        };
    }
}