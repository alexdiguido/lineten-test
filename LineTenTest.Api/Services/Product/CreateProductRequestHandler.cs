using System.Net;
using Ardalis.GuardClauses;
using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Utilities;
using LineTenTest.Api.Utilities.Mappers;
using LineTenTest.Domain.Services.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotFoundException = LineTenTest.Domain.Exceptions.NotFoundException;

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

        public async Task<ActionResult<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product? productResult;
            try
            {
                Guard.Against.Null(request.Request);
                Guard.Against.NullOrEmpty(request.Request.Description);
                Guard.Against.NullOrEmpty(request.Request.Name);
                Guard.Against.NullOrEmpty(request.Request.Sku);
                productResult = await _service.CreateAsync(request.Request);
            }
            catch (ArgumentException argumentEx)
            {
                return new BadRequestObjectResult(argumentEx.Message);
            }
            catch (NotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, request);
                return new ObjectResult(Constants.InternalServerErrorResultMessage)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            return new OkObjectResult(ProductDtoMapper.MapFrom(productResult));
        }
    }
}
