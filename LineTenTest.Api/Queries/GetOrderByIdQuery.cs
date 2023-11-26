using LineTenTest.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Queries;

public class GetOrderByIdQuery : IRequest<ActionResult<OrderDto>>
{
    public int OrderId { get; }

    public GetOrderByIdQuery(int orderId)
    {
        OrderId = orderId;
    }
}