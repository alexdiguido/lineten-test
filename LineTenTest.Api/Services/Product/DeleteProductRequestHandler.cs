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
    public class DeleteProductRequestHandler: IRequestHandler<DeleteProductCommand,IActionResult>
    {
        private readonly IDeleteProductService _service;
        private readonly ILogger<DeleteProductRequestHandler> _logger;

        public DeleteProductRequestHandler(IDeleteProductService service, ILogger<DeleteProductRequestHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product? productResult;
            try
            {
                Guard.Against.Negative(request.Request.ProductId);
                await _service.DeleteAsync(request.Request);
            }
            catch (ArgumentException argumentEx)
            {
                return new BadRequestObjectResult(argumentEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, request);
                return new ObjectResult(Constants.InternalServerErrorResultMessage)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            return new OkResult();
        }
    }
}
