using LineTenTest.Domain.Exceptions;
using LineTenTest.SharedKernel;
using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Domain.Services.Product;

public class UpdateProductService : IUpdateProductService
{
    private readonly IRepository<Entities.Product> _repository;

    public UpdateProductService(IRepository<Entities.Product> repository)
    {
        _repository = repository;
    }
    public async Task<Entities.Product> UpdateAsync(UpdateProductRequest request)
    {
        var product = await _repository.FirstOrDefaultAsync(new GetProductToUpdateSpecification(request.ProductId));

        if (product == null)
        {
            throw new EntityNotFoundException("product not found");
        }

        product.SKU = request.Sku;
        product.Description = request.Description;
        product.Name = request.Name;

        await _repository.SaveChangesAsync();

        return product;
    }
}