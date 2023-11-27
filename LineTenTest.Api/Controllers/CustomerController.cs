using Asp.Versioning;
using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Queries;
using LineTenTest.Domain.Entities;
using LineTenTest.SharedKernel.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace LineTenTest.Api.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class CustomerController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IMediator mediator, ILogger<CustomerController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomerDto>> Get([Required]int customerId)
        {
            return await HandleOperationAsync(async () =>
                await _mediator.Send(new GetCustomerByIdQuery(customerId)));
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomerDto>> Create(CreateCustomerRequest createCustomerRequest)
        {
            return await HandleOperationAsync(async () =>
                await _mediator.Send(new CreateCustomerCommand(createCustomerRequest)));
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomerDto>> Update(UpdateCustomerRequest updateCustomerRequest)
        {
            return await HandleOperationAsync(async () =>
                await _mediator.Send(new UpdateCustomerCommand(updateCustomerRequest)));
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromQuery]DeleteCustomerRequest deleteCustomerRequest)
        {
            try
            {
                return await _mediator.Send(new DeleteCustomerCommand(deleteCustomerRequest));
            }
            catch (Exception ex)
            {
                var message = "an error occurred. Please contact Application admin";
                return new ObjectResult(message) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }

        private async Task<ActionResult<CustomerDto>> HandleOperationAsync(
            Func<Task<ActionResult<CustomerDto>>> operation)
        {
            try
            {
                return await operation.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                var message = "an error occurred. Please contact Application admin";
                return new ObjectResult(message) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
