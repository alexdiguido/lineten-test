using LineTenTest.Api.Dtos;
using LineTenTest.Api.Queries;
using LineTenTest.Domain.Services.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Services.Product
{
    public class GetProductRequestHandler : IRequestHandler<GetProductByIdQuery,ActionResult<ProductDto>>
    {
        private readonly Logger<GetProductRequestHandler> _logger;

        public GetProductRequestHandler(IGetProductService service, Logger<GetProductRequestHandler> logger)
        {
            _logger = logger;
        }
        public Task<ActionResult<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
