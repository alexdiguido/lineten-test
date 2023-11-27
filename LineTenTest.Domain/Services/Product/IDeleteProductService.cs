using LineTenTest.Api.Commands;

namespace LineTenTest.Domain.Services.Product;

public interface IDeleteProductService
{
    Task DeleteAsync(DeleteProductRequest request);
}