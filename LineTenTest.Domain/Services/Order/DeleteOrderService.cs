using LineTenTest.Domain.Exceptions;
using LineTenTest.SharedKernel;
using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Domain.Services.Order;

public class DeleteOrderService : IDeleteOrderService
{
    private readonly IRepository<Entities.Order> _orderRepository;

    public DeleteOrderService(IRepository<Entities.Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task DeleteAsync(DeleteOrderRequest request)
    {
        var order = await _orderRepository.FirstOrDefaultAsync(new GetOrderByIdSpecificationToUpdate(request.OrderId));
        if (order == null)
        {
            throw new EntityNotFoundException("Order Not Found");
        }

        await _orderRepository.DeleteAsync(order);
        await _orderRepository.SaveChangesAsync();
    }
}