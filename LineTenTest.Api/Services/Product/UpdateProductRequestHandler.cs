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
        Guard.Against.Null(request.Request);
        Guard.Against.NegativeOrZero(request.Request.ProductId);
        Guard.Against.NullOrEmpty(request.Request.Description);
        Guard.Against.NullOrEmpty(request.Request.Name);
        Guard.Against.NullOrEmpty(request.Request.Sku);
        return async() => await _service.UpdateAsync(request.Request);
    }
}