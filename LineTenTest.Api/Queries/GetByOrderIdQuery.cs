using LineTenTest.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Queries;

public class GetByOrderIdQuery : IRequest<ActionResult<OrderDto>>
{
    public int OrderId { get; }

    public GetByOrderIdQuery(int orderId)
    {
        OrderId = orderId;
    }
}