using Ardalis.Specification;

namespace LineTenTest.Domain.Services.Order;

public class GetOrderByIdSpecificationToUpdate : Specification<Entities.Order>
{
    public GetOrderByIdSpecificationToUpdate(int orderId)
    {
        Query.Where(order => order.Id == orderId)
            .Include(order => order.Customer)
            .Include(order => order.Product);
    }
}