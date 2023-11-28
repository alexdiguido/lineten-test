using LineTenTest.Domain.Services.Customer;
using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Domain.Services.Product;

public interface IUpdateProductService
{
    Task<Entities.Product> UpdateAsync(UpdateProductRequest request);
}