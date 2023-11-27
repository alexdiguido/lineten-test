using LineTenTest.Api.Controllers;
using LineTenTest.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Commands;

public class CreateCustomerCommand : IRequest<ActionResult<CustomerDto>>
{
    public CreateCustomerRequest Request { get; }

    public CreateCustomerCommand(CreateCustomerRequest request)
    {
        Request = request;
    }
}