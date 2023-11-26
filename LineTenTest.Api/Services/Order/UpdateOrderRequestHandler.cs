using Ardalis.GuardClauses;
using LineTenTest.Api.Commands;
using LineTenTest.Domain.Entities;
using LineTenTest.Domain.Services.Order;

namespace LineTenTest.Api.Services.Order
{
    public class UpdateOrderRequestHandler : BaseOrderRequestHandler<UpdateOrderCommand>
    {
        private readonly IUpdateOrderService _service;

        public UpdateOrderRequestHandler(IUpdateOrderService service, ILogger<UpdateOrderRequestHandler> logger) : base(logger)
        {
            _service = service;
        }

        protected override Func<Task<Domain.Entities.Order>> ExecuteServiceOperation(UpdateOrderCommand request)
        {
            Guard.Against.Null(request.Request);
            Guard.Against.Negative(request.Request.CustomerId);
            Guard.Against.Negative(request.Request.ProductId);
            Guard.Against.Negative(request.Request.OrderId);
            Guard.Against.EnumOutOfRange<EOrderStatus>(request.Request.Status);
            return async () => await _service.UpdateAsync(request.Request);
        }
    }
}
