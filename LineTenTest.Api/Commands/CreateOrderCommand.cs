using LineTenTest.Api.Dtos;
using LineTenTest.SharedKernel.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Commands;

public class CreateOrderCommand : IRequest<ActionResult<OrderDto>>
{
    public CreateOrderRequest CreateOrderRequest { get; }

    public CreateOrderCommand(CreateOrderRequest createOrderRequest)
    {
        CreateOrderRequest = createOrderRequest;
    }
}