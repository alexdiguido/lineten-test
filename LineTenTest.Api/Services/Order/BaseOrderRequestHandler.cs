using System.Net;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Utilities;
using LineTenTest.Api.Utilities.Mappers;
using LineTenTest.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Services.Order
{
    public abstract class BaseOrderRequestHandler<TRequest> : IRequestHandler<TRequest, ActionResult<OrderDto>>
            where TRequest : IRequest<ActionResult<OrderDto>>
    {
        private readonly ILogger<BaseOrderRequestHandler<TRequest>> _logger;

        protected BaseOrderRequestHandler(ILogger<BaseOrderRequestHandler<TRequest>> logger)
        {
            _logger = logger;
        }

        protected abstract Func<Task<Domain.Entities.Order>> ExecuteServiceOperation(TRequest request);

        public async Task<ActionResult<OrderDto>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Order? orderResult;
            try
            {
                orderResult = await ExecuteServiceOperation(request)();
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

            return new OkObjectResult(OrderDtoMapper.MapFrom(orderResult));
        }
    }
}
