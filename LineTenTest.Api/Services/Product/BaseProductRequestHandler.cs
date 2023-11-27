using Ardalis.GuardClauses;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Utilities.Mappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using LineTenTest.Api.Utilities;
using LineTenTest.Api.Services.Order;

namespace LineTenTest.Api.Services.Product
{
    public abstract class BaseProductRequestHandler<TRequest> : IRequestHandler<TRequest, ActionResult<ProductDto>>
        where TRequest : IRequest<ActionResult<ProductDto>>
    {
        private readonly ILogger<BaseProductRequestHandler<TRequest>> _logger;

        public BaseProductRequestHandler(ILogger<BaseProductRequestHandler<TRequest>> logger)
        {
            _logger = logger;
        }

        protected abstract Func<Task<Domain.Entities.Product>> ExecuteServiceOperation(TRequest request);

        public async Task<ActionResult<ProductDto>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product? productResult;
            try
            {
                productResult = await ExecuteServiceOperation(request)();
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
