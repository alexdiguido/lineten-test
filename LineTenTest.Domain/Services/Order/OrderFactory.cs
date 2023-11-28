using LineTenTest.Domain.Entities;
using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Domain.Services.Order;

public class OrderFactory
{
    public static Entities.Order CreateOrder(CreateOrderRequest createOrderRequest)
    {
        return new Entities.Order()
        {
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
            Status = (int)EOrderStatus.Processing,
            CustomerId = createOrderRequest.CustomerId,
            ProductId = createOrderRequest.ProductId
        };
    }
    public static Entities.Order CreateOrder(UpdateOrderRequest createOrderRequest)
    {
        return new Entities.Order()
        {
            UpdatedDate = DateTime.Now,
            Status = createOrderRequest.Status,
            CustomerId = createOrderRequest.CustomerId,
            ProductId = createOrderRequest.ProductId
        };
    }
    
}