using LineTenTest.Api.Dtos;
using LineTenTest.Domain.Entities;

namespace LineTenTest.Api.Utilities.Mappers;

public class OrderDtoMapper
{
    public static OrderDto MapFrom(Order orderResult)
    {
        return new OrderDto()
        {
            OrderId = orderResult.Id,
            Status = orderResult.Status,
            CreatedDate = orderResult.CreatedDate,
            UpdateDate = orderResult.UpdatedDate,
            Customer = CustomerDtoMapper.MapFrom(orderResult.Customer),
            Product = ProductDtoMapper.MapFrom(orderResult.Product),
        };
    }
}