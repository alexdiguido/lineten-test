using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Domain.Services.Order;

public interface IUpdateOrderService
{
    Task<Entities.Order> UpdateAsync(UpdateOrderRequest request);
}