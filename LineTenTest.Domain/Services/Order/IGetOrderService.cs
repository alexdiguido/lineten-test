namespace LineTenTest.Domain.Services.Order
{
    public interface IGetOrderService
    {
        Task<Entities.Order> GetAsync(int orderId);
    }
}
