using LineTenTest.Api.Dtos;
using LineTenTest.SharedKernel.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Commands;

public class DeleteOrderCommand : IRequest<IActionResult>
{
    public DeleteOrderRequest Request { get; }

    public DeleteOrderCommand(DeleteOrderRequest request)
    {
        Request = request;
    }
}