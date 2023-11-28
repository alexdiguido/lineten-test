using Ardalis.Specification;

namespace LineTenTest.Domain.Services.Product;

public class GetProductToUpdateSpecification : Specification<Entities.Product>
{
    public GetProductToUpdateSpecification(int productId)
    {
        Query.Where(p => p.Id == productId);
    }
}