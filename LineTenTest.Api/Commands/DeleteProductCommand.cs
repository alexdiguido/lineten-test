using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Commands;

public class DeleteProductCommand : IRequest<IActionResult>
{
    public DeleteProductRequest Request { get; }

    public DeleteProductCommand(DeleteProductRequest request)
    {
        Request = request;
    }
}