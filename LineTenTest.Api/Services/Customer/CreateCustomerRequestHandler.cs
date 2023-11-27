using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Utilities.Mappers;
using LineTenTest.Domain.Services.Customer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ardalis.GuardClauses;
using LineTenTest.Api.Extensions;
using LineTenTest.Api.Utilities;

namespace LineTenTest.Api.Services.Customer
{
    public class CreateCustomerRequestHandler : IRequestHandler<CreateCustomerCommand,ActionResult<CustomerDto>>
    {
        private readonly ICreateCustomerService _service;
        private readonly ILogger<CreateCustomerRequestHandler> _logger;

        public CreateCustomerRequestHandler(ICreateCustomerService service, ILogger<CreateCustomerRequestHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<ActionResult<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Customer? productResult;
            try
            {
                Guard.Against.Null(request.Request);
                Guard.Against.NullOrWhiteSpace(request.Request.FirstName);
                Guard.Against.NullOrWhiteSpace(request.Request.LastName);
                Guard.Against.InvalidEmail(request.Request.Email, nameof(request.Request.Email));
                Guard.Against.NullOrWhiteSpace(request.Request.Phone);
                productResult = await _service.CreateAsync(request.Request);
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
