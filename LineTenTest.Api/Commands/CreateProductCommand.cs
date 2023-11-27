using LineTenTest.Api.Dtos;
using LineTenTest.SharedKernel.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Commands;

public class CreateProductCommand : IRequest<ActionResult<ProductDto>>
{
    public CreateProductRequest Request { get; }

    public CreateProductCommand(CreateProductRequest request)
    {
        Request = request;
    }
}