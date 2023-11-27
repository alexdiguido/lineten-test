using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Domain.Services.Product;

public interface ICreateProductService
{
    Task<Entities.Product> CreateAsync(CreateProductRequest request);
}