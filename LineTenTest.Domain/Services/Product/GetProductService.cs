using LineTenTest.Domain.Exceptions;
using LineTenTest.SharedKernel;

namespace LineTenTest.Domain.Services.Product;

public class GetProductService : IGetProductService
{
    private readonly IRepository<Entities.Product> _repository;

    public GetProductService(IRepository<Entities.Product> repository)
    {
        _repository = repository;
    }

    public async Task<Entities.Product> GetAsync(int productId)
    {
        var productResult = await _repository.FirstOrDefaultAsync(new GetProductSpecification(productId));
        return productResult ?? throw new EntityNotFoundException("Product Not found");
    }
}