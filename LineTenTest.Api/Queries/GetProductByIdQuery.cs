using LineTenTest.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Queries;

public class GetProductByIdQuery : IRequest<ActionResult<OrderDto>>
{
    public int ProductId { get; }

    public GetProductByIdQuery(int productId)
    {
        ProductId = productId;
    }
}