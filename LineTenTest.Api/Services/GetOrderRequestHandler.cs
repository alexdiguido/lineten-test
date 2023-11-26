using LineTenTest.Api.Dtos;
using LineTenTest.Api.Queries;
using LineTenTest.Domain.Services.Order;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Services
{
    public class GetOrderRequestHandler : IRequestHandler<GetOrderByIdQuery,ActionResult<OrderDto>>
    {
        private readonly IGetOrderService _service;

        public GetOrderRequestHandler(IGetOrderService service)
        {
            _service = service;
        }

        public Task<ActionResult<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
