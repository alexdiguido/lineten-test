using Ardalis.Specification;

namespace LineTenTest.Domain.Services.Product;

public class GetProductSpecification : Specification<Entities.Product>
{
    public GetProductSpecification(int productId)
    {
        Query.Where(product => product.Id == productId)
            .AsNoTracking();
    }
}