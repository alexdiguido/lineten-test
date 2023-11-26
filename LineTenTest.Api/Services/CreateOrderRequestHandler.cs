using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Services
{
    public class CreateOrderRequestHandler : IRequestHandler<CreateOrderCommand,ActionResult<OrderDto>>
    {
        public CreateOrderRequestHandler(ICreateOrderService service, ILogger<CreateOrderRequestHandler> logger)
        {
            
        }

        public Task<ActionResult<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
