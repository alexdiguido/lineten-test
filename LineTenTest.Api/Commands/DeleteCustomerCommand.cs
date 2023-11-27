using LineTenTest.Api.Controllers;
using LineTenTest.SharedKernel.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Commands;

public class DeleteCustomerCommand : IRequest<IActionResult>
{
    public DeleteCustomerRequest Request { get; }

    public DeleteCustomerCommand(DeleteCustomerRequest request)
    {
        Request = request;
    }
}