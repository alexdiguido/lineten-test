using LineTenTest.Api.Dtos;
using LineTenTest.Api.Queries;
using LineTenTest.Api.Utilities.Mappers;
using LineTenTest.Domain.Exceptions;
using LineTenTest.Domain.Services.Order;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Services
{
    public class GetOrderRequestHandler : IRequestHandler<GetOrderByIdQuery,ActionResult<OrderDto>>
    {
        private readonly IGetOrderService _service;
        private readonly ILogger<GetOrderRequestHandler> _logger;

        public GetOrderRequestHandler(IGetOrderService service, ILogger<GetOrderRequestHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<ActionResult<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var orderResult = await _service.GetAsync(request.OrderId);

            return new OkObjectResult(OrderDtoMapper.MapFrom(orderResult));
        }
    }
}
