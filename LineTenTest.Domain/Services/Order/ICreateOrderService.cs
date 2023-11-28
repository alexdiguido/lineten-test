using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Domain.Services.Order;

public interface ICreateOrderService
{
    Task<Entities.Order> CreateAsync(CreateOrderRequest createOrderRequest);
}