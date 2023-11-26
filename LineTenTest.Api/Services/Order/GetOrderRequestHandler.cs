using Ardalis.GuardClauses;
using LineTenTest.Api.Queries;
using LineTenTest.Domain.Services.Order;

namespace LineTenTest.Api.Services.Order
{
    public class GetOrderRequestHandler : BaseOrderRequestHandler<GetOrderByIdQuery>
    {
        private readonly IGetOrderService _service;

        public GetOrderRequestHandler(IGetOrderService service, ILogger<GetOrderRequestHandler> logger) : base(logger)
        {
            _service = service;
        }

        protected override Func<Task<Domain.Entities.Order>> ExecuteServiceOperation(GetOrderByIdQuery request)
        {
            Guard.Against.Negative(request.OrderId);
            return async () => await _service.GetAsync(request.OrderId);
        }
    }
}
