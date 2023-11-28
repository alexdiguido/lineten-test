using LineTenTest.Api.Commands;
using LineTenTest.Domain.Exceptions;
using LineTenTest.SharedKernel;

namespace LineTenTest.Domain.Services.Product;

public class DeleteProductService : IDeleteProductService
{
    private readonly IRepository<Entities.Product> _repository;

    public DeleteProductService(IRepository<Entities.Product> repository)
    {
        _repository = repository;
    }

    public async Task DeleteAsync(DeleteProductRequest request)
    {
        var product = await _repository.FirstOrDefaultAsync(new GetProductToUpdateSpecification(request.ProductId));
        if (product == null)
        {
            throw new NotFoundException("Product not found");
        }
        await _repository.DeleteAsync(product);
        await _repository.SaveChangesAsync();
    }
}