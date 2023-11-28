using Ardalis.GuardClauses;
using LineTenTest.Api.Commands;
using LineTenTest.Domain.Services.Customer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using LineTenTest.Api.Utilities;

namespace LineTenTest.Api.Services.Customer
{
    public class DeleteCustomerRequestHandler : IRequestHandler<DeleteCustomerCommand,IActionResult>
    {
        private readonly IDeleteCustomerService _service;
        private readonly ILogger<DeleteCustomerRequestHandler> _logger;

        public DeleteCustomerRequestHandler(IDeleteCustomerService service, ILogger<DeleteCustomerRequestHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Guard.Against.Negative(request.Request.CustomerId);
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
