using LineTenTest.Domain.Entities;
using LineTenTest.SharedKernel;

namespace LineTenTest.Domain.Services;

public class GetOrderService : IGetOrderService
{
    private readonly IRepository<Order> _repository;

    public GetOrderService(IRepository<Order> repository)
    {
        _repository = repository;
    }

    public Task<Order> GetAsync(int orderId)
    {
        throw new NotImplementedException();
    }
}