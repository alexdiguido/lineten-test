using System.Net;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Utilities;
using LineTenTest.Api.Utilities.Mappers;
using LineTenTest.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Services.Customer
{
    public abstract class BaseCustomerRequestHandler<TRequest> : IRequestHandler<TRequest, ActionResult<CustomerDto>>
        where TRequest : IRequest<ActionResult<CustomerDto>>
    {
        private readonly ILogger<BaseCustomerRequestHandler<TRequest>> _logger;

        protected BaseCustomerRequestHandler(ILogger<BaseCustomerRequestHandler<TRequest>> logger)
        {
            _logger = logger;
        }

        protected abstract Func<Task<Domain.Entities.Customer>> ExecuteServiceOperation(TRequest request);

        public async Task<ActionResult<CustomerDto>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Customer? customerResult;
            try
            {
                customerResult = await ExecuteServiceOperation(request)();
            }
            catch (ArgumentException argumentEx)
            {
                return new BadRequestObjectResult(argumentEx.Message);
            }
            catch (EntityNotFoundException)
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

            return new OkObjectResult(CustomerDtoMapper.MapFrom(customerResult));
        }
    }
}
