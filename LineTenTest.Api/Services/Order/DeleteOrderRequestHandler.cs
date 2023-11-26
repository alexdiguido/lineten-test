using LineTenTest.Api.Commands;
using LineTenTest.Api.Utilities.Mappers;
using LineTenTest.Domain.Services.Order;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ardalis.GuardClauses;
using LineTenTest.Api.Utilities;
using LineTenTest.Domain.Exceptions;

namespace LineTenTest.Api.Services.Order
{
    public class DeleteOrderRequestHandler : IRequestHandler<DeleteOrderCommand,IActionResult>
    {
        private readonly IDeleteOrderService _service;
        private readonly ILogger<DeleteOrderRequestHandler> _logger;

        public DeleteOrderRequestHandler(IDeleteOrderService service, ILogger<DeleteOrderRequestHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Guard.Against.Null(request.Request);
                Guard.Against.Negative(request.Request.OrderId);
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
