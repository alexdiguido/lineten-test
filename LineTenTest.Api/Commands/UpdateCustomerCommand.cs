using LineTenTest.Api.Dtos;
using LineTenTest.SharedKernel.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Commands;

public class UpdateCustomerCommand : IRequest<ActionResult<CustomerDto>>
{
    public UpdateCustomerRequest Request { get; }

    public UpdateCustomerCommand(UpdateCustomerRequest request)
    {
        Request = request;
    }
}