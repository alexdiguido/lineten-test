using System.Net;
using Ardalis.GuardClauses;
using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Utilities;
using LineTenTest.Api.Utilities.Mappers;
using LineTenTest.Domain.Services.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Services.Product
{
    public class CreateProductRequestHandler :BaseProductRequestHandler<CreateProductCommand>
    {
        private readonly ICreateProductService _service;

        public CreateProductRequestHandler(ICreateProductService service, ILogger<CreateProductRequestHandler> logger) : base(logger)
        {
            _service = service;
        }

        protected override Func<Task<Domain.Entities.Product>> ExecuteServiceOperation(CreateProductCommand request)
        {
            Guard.Against.Null(request.Request);
            Guard.Against.NullOrEmpty(request.Request.Description);
            Guard.Against.NullOrEmpty(request.Request.Name);
            Guard.Against.NullOrEmpty(request.Request.Sku);
            return async() => await _service.CreateAsync(request.Request);
        }
    }
}
