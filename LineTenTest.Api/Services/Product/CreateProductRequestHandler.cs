using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Domain.Services.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Services.Product
{
    public class CreateProductRequestHandler : IRequestHandler<CreateProductCommand,ActionResult<ProductDto>>
    {
        private readonly ICreateProductService _service;
        private readonly ILogger<CreateProductRequestHandler> _logger;

        public CreateProductRequestHandler(ICreateProductService service, ILogger<CreateProductRequestHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public Task<ActionResult<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
