using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Domain.Services.Order;

public interface IDeleteOrderService
{
    Task DeleteAsync(DeleteOrderRequest request);
}