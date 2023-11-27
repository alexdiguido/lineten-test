namespace LineTenTest.Domain.Services.Product;

public interface IGetProductService
{
    Task<Entities.Product> GetAsync(int productId);
}