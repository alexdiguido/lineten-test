using LineTenTest.Api.Dtos;
using LineTenTest.SharedKernel.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Commands;

public class UpdateOrderCommand : IRequest<ActionResult<OrderDto>>
{
    public UpdateOrderRequest Request { get; }

    public UpdateOrderCommand(UpdateOrderRequest request)
    {
        Request = request;
    }
}