using LineTenTest.SharedKernel;
using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Domain.Services.Product;

public class CreateProductService : ICreateProductService
{
    private readonly IRepository<Entities.Product> _repository;

    public CreateProductService(IRepository<Entities.Product> repository)
    {
        _repository = repository;
    }

    public Task<Entities.Product> CreateAsync(CreateProductRequest request)
    {
        var product = ProductFactory.CreateProduct(request);
        var productResult = _repository.AddAsync(product);
        _repository.SaveChangesAsync();
        return productResult;
    }
}