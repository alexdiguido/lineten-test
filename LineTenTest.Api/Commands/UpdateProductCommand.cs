using LineTenTest.Api.Controllers;
using LineTenTest.Api.Dtos;
using LineTenTest.SharedKernel.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Commands;

public class UpdateProductCommand : IRequest<ActionResult<ProductDto>>
{
    public UpdateProductRequest Request { get; }

    public UpdateProductCommand(UpdateProductRequest request)
    {
        Request = request;
    }
}