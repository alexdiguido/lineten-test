using System.Net.Mime;
using Asp.Versioning;
using LineTenTest.Api.Commands;
using LineTenTest.Api.Dtos;
using LineTenTest.Api.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LineTenTest.Api.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderDto>> Get(int orderId)
        {
            return await HandleOrderOperationAsync(async () =>
                await _mediator.Send(new GetOrderByIdQuery(orderId)));
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderDto>> Create(CreateOrderDto createOrderDto)
        {
            return await HandleOrderOperationAsync(async () =>
                await _mediator.Send(new CreateOrderCommand(createOrderDto)));
        }

        public async Task<ActionResult<OrderDto>> HandleOrderOperationAsync(
            Func<Task<ActionResult<OrderDto>>> operation)
        {
            try
            {
                return await operation.Invoke();
            }
            catch (Exception ex)
            {
                var message = "an error occurred. Please contact Application admin";
                return new ObjectResult(message) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
