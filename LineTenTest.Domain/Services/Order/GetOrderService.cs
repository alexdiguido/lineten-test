using LineTenTest.Domain.Entities;
using LineTenTest.Domain.Exceptions;
using LineTenTest.SharedKernel;
using LineTenTest.Domain.Entities;

namespace LineTenTest.Domain.Services.Order;

public class GetOrderService : IGetOrderService
{
    private readonly IRepository<Entities.Order> _repository;

    public GetOrderService(IRepository<Entities.Order> repository)
    {
        _repository = repository;
    }

    public async Task<Entities.Order> GetAsync(int orderId)
    {
        throw new NotImplementedException();
    }
}