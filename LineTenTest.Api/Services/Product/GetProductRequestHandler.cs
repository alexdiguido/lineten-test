using Ardalis.GuardClauses;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Queries;
using LineTenTest.Domain.Services.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Services.Product
{
    public class GetProductRequestHandler : BaseProductRequestHandler<GetProductByIdQuery>
    {
        private readonly IGetProductService _service;

        public GetProductRequestHandler(IGetProductService service, ILogger<GetProductRequestHandler> logger) : base(logger)
        {
            _service = service;
        }

        protected override Func<Task<Domain.Entities.Product>> ExecuteServiceOperation(GetProductByIdQuery request)
        {
            Guard.Against.NegativeOrZero(request.ProductId);
            return async () => await _service.GetAsync(request.ProductId);
        }
    }
}
