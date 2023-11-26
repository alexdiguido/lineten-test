using LineTenTest.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Commands;

public class CreateOrderCommand : IRequest<ActionResult<OrderDto>>
{
    public CreateOrderDto CreateOrderDto { get; }

    public CreateOrderCommand(CreateOrderDto createOrderDto)
    {
        CreateOrderDto = createOrderDto;
    }
}