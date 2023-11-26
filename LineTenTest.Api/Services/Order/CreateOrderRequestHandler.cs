using Ardalis.GuardClauses;
using LineTenTest.Api.Commands;
using LineTenTest.Domain.Services.Order;

namespace LineTenTest.Api.Services.Order
{
    public class CreateOrderRequestHandler : BaseOrderRequestHandler<CreateOrderCommand>
    {
        private readonly ICreateOrderService _service;

        public CreateOrderRequestHandler(ICreateOrderService service, ILogger<CreateOrderRequestHandler> logger) : base(logger)
        {
            _service = service;
        }

        protected override Func<Task<Domain.Entities.Order>> ExecuteServiceOperation(CreateOrderCommand request)
        {
            Guard.Against.Null(request.CreateOrderRequest);
            Guard.Against.Negative(request.CreateOrderRequest.CustomerId);
            Guard.Against.Negative(request.CreateOrderRequest.ProductId);
            return async () => await _service.CreateAsync(request.CreateOrderRequest);
        }
    }
}
