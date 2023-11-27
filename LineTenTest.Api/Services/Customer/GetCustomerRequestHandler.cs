using Ardalis.GuardClauses;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Queries;
using LineTenTest.Api.Utilities.Mappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using LineTenTest.Api.Extensions;
using LineTenTest.Api.Utilities;

namespace LineTenTest.Api.Services.Customer
{
    public class GetCustomerRequestHandler : IRequestHandler<GetCustomerByIdQuery,ActionResult<CustomerDto>>
    {
        private readonly IGetCustomerService _service;
        private readonly ILogger<GetCustomerRequestHandler> _logger;

        public GetCustomerRequestHandler(IGetCustomerService service, ILogger<GetCustomerRequestHandler> logger)
        {
            _service = service;
            _logger = logger;
        }
        public async Task<ActionResult<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            Domain.Entities.Customer? productResult;
            try
            {
                Guard.Against.Negative(request.CustomerId);
                productResult = await _service.GetAsync(request.CustomerId);
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

            return new OkObjectResult(CustomerDtoMapper.MapFrom(productResult));
        }
    }
}
