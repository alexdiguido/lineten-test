using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Domain.Services.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Services.Product
{
    public class DeleteProductRequestHandler: IRequestHandler<DeleteProductCommand,IActionResult>
    {
        private readonly IDeleteProductService _service;

        public DeleteProductRequestHandler(IDeleteProductService service, ILogger<DeleteProductRequestHandler> logger)
        {
            _service = service;
        }

        public Task<IActionResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
