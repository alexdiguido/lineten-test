using Ardalis.GuardClauses;
using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Domain.Services.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Services.Product;

public class UpdateProductRequestHandler : BaseProductRequestHandler<UpdateProductCommand>
{
    private readonly IUpdateProductService _service;

    public UpdateProductRequestHandler(IUpdateProductService service, ILogger<UpdateProductRequestHandler> logger) : base(logger)
    {
        _service = service;
    }

    protected override Func<Task<Domain.Entities.Product>> ExecuteServiceOperation(UpdateProductCommand request)
    {
       throw new NotImplementedException();
    }
}