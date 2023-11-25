using Ardalis.Specification;
using LineTenTest.Domain.Entities;

namespace LineTenTest.Domain.Services.Order;

public class GetOrderByIdSpecification : Specification<Entities.Order>
{
    public GetOrderByIdSpecification(int orderId)
    {
        Query.Where(order => order.Id == orderId)
            .Include(order => order.Customer)
            .Include(order => order.Product)
            .AsNoTracking();
    }
}